using ComprobantePago.Application.Common;
using ComprobantePago.Application.Interfaces;
using ComprobantePago.Application.Interfaces.QueryServices;
using ComprobantePago.Application.Interfaces.Repositories;
using ComprobantePago.Application.Interfaces.Services;
using ComprobantePago.Application.Interfaces.Services.Maestros;
using ComprobantePago.Application.Mapping;
using ComprobantePago.Application.Settings;
using ComprobantePago.Application.Validations;
using ComprobantePago.Infrastructure.Persistence;
using ComprobantePago.Infrastructure.QueryServices;
using ComprobantePago.Infrastructure.Repositories;
using ComprobantePago.Infrastructure.Services;
using ComprobantePago.Infrastructure.Services.Maestros;
using ComprobantePago.Web.Middlewares;
using FluentValidation;
using Mapster;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using Serilog.Events;
using System.Text.Json;

// ── Serilog: configurar antes del builder ─────────────────────────────────────
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
    .MinimumLevel.Override("Microsoft.EntityFrameworkCore.Database.Command",
        LogEventLevel.Warning)
    .Enrich.FromLogContext()
    .Enrich.WithMachineName()
    .WriteTo.Console(
        outputTemplate:
        "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}")
    .WriteTo.File(
        path: "logs/comprobante-.log",
        rollingInterval: RollingInterval.Day,
        retainedFileCountLimit: 30,
        outputTemplate:
        "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {Message:lj}{NewLine}{Exception}")
    .CreateLogger();

try
{
    Log.Information("Iniciando aplicación ComprobantePago...");

    var builder = WebApplication.CreateBuilder(args);

    // ── Serilog ───────────────────────────────────────────────────────────────
    builder.Host.UseSerilog();

    // ── MVC + JSON camelCase ──────────────────────────────────────────────────
    builder.Services.AddControllersWithViews()
        .AddJsonOptions(options =>
        {
            options.JsonSerializerOptions.PropertyNamingPolicy    = JsonNamingPolicy.CamelCase;
            options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
        });

    // ── Swagger / OpenAPI ─────────────────────────────────────────────────────
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen(c =>
    {
        c.SwaggerDoc("v1", new()
        {
            Title       = "ComprobantePago API",
            Version     = "v1",
            Description = "API REST para la gestión de comprobantes de pago."
        });
    });

    // ── Base de datos MySQL ───────────────────────────────────────────────────
    builder.Services.AddDbContext<AppDbContext>(options =>
        options.UseMySql(
            builder.Configuration.GetConnectionString("DefaultConnection"),
            ServerVersion.AutoDetect(
                builder.Configuration.GetConnectionString("DefaultConnection"))
        )
    );

    // ── Configuración SUNAT ───────────────────────────────────────────────────
    builder.Services.Configure<SunatSettings>(
        builder.Configuration.GetSection("Sunat"));

    // ── Configuración Empresa ─────────────────────────────────────────────────
    builder.Services.Configure<EmpresaSettings>(
        builder.Configuration.GetSection(EmpresaSettings.Section));

    // ── HttpClient SUNAT ──────────────────────────────────────────────────────
    builder.Services.AddHttpClient<ISunatService, SunatService>()
        .ConfigurePrimaryHttpMessageHandler(() =>
        {
            var handler = new HttpClientHandler { UseProxy = false };
            // Omitir validación SSL solo en desarrollo (SUNAT Beta usa cert autofirmado).
            // En producción se valida el certificado normalmente.
            if (builder.Environment.IsDevelopment())
            {
                handler.ServerCertificateCustomValidationCallback =
                    HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;
            }
            return handler;
        });

    // ── Mapster: configurar mappings de la capa Application ──────────────────
    MapsterConfig.Configure();

    // ── FluentValidation ──────────────────────────────────────────────────────
    builder.Services.AddValidatorsFromAssemblyContaining<RegistrarComprobanteValidator>();

    // ── Configuración ApiMaestros ─────────────────────────────────────────────
    builder.Services.Configure<ApiMaestrosSettings>(
        builder.Configuration.GetSection(ApiMaestrosSettings.Section));

    var usarApiMaestros = builder.Configuration
        .GetValue<bool>($"{ApiMaestrosSettings.Section}:UsarApi");

    if (usarApiMaestros)
    {
        builder.Services.AddHttpClient<IEmpleadoService, ApiEmpleadoService>();
        builder.Services.AddHttpClient<IProveedorService, ApiProveedorService>();
        builder.Services.AddHttpClient<ICatalogoUnidadService, ApiCatalogoUnidadService>();
        builder.Services.AddHttpClient<ICuentaContableService, ApiCuentaContableService>();
    }
    else
    {
        builder.Services.AddScoped<IEmpleadoService, DbEmpleadoService>();
        builder.Services.AddScoped<IProveedorService, DbProveedorService>();
        builder.Services.AddScoped<ICatalogoUnidadService, DbCatalogoUnidadService>();
        builder.Services.AddScoped<ICuentaContableService, DbCuentaContableService>();
    }

    // ── Autenticación JWT / Azure Entra ID (multi-tenant) ────────────────────
    // Sin [Authorize] en los controladores, los requests anónimos pasan sin problema.
    // Cuando se activen los tenants reales, solo se necesita completar ValidTenants en appsettings.
    var azureAdConfig = builder.Configuration.GetSection("AzureAd");
    var validTenants  = azureAdConfig.GetSection("ValidTenants").Get<string[]>() ?? [];

    builder.Services.AddHttpContextAccessor();
    builder.Services.AddScoped<IUsuarioContexto, UsuarioContexto>();

    builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddJwtBearer(options =>
        {
            options.Authority = $"{azureAdConfig["Instance"]}common/v2.0";
            options.Audience  = azureAdConfig["Audience"];

            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer           = validTenants.Length > 0,
                ValidateAudience         = !string.IsNullOrWhiteSpace(azureAdConfig["Audience"]),
                ValidateLifetime         = true,
                ValidateIssuerSigningKey = true,

                // Acepta únicamente los tenants configurados (GCI, Los Andes, Tecsur)
                IssuerValidator = (issuer, _, _) =>
                {
                    if (validTenants.Length == 0) return issuer; // modo desarrollo sin tenants

                    var issuersPermitidos = validTenants
                        .Select(t => $"https://login.microsoftonline.com/{t}/v2.0");

                    if (issuersPermitidos.Contains(issuer)) return issuer;

                    throw new SecurityTokenInvalidIssuerException(
                        $"Tenant no autorizado: {issuer}");
                }
            };
        });

    // ── Servicios de aplicación ───────────────────────────────────────────────
    builder.Services.AddScoped<XmlComprobanteService>();
    builder.Services.AddScoped<PdfComprobanteService>();
    builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
    builder.Services.AddScoped<IComprobanteQueryService, ComprobanteQueryService>();
    builder.Services.AddScoped<ISytelineQueryService, SytelineQueryService>();
    builder.Services.AddScoped<IMaestrosQueryService, MaestrosQueryService>();
    builder.Services.AddScoped<IComprobanteRepository, ComprobanteRepository>();
    builder.Services.AddScoped<IExcelSytelineService, ExcelSytelineService>();

    var app = builder.Build();

    // ── Columnas pendientes de BD (idempotente, IF NOT EXISTS) ────────────────
    using (var scope = app.Services.CreateScope())
    {
        var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        await db.Database.ExecuteSqlRawAsync(@"
            ALTER TABLE rcocomprobante
                ADD COLUMN IF NOT EXISTS FechaDigitacion   DATETIME NULL,
                ADD COLUMN IF NOT EXISTS FechaAutorizacion DATETIME NULL");
    }

    // ── Pipeline de middlewares ───────────────────────────────────────────────
    // 1. Manejo global de excepciones (primero)
    app.UseMiddleware<ExceptionMiddleware>();

    // 2. Serilog request logging
    app.UseSerilogRequestLogging(opts =>
    {
        opts.MessageTemplate =
            "HTTP {RequestMethod} {RequestPath} respondió {StatusCode} en {Elapsed:0.0000} ms";
    });

    if (!app.Environment.IsDevelopment())
    {
        app.UseExceptionHandler("/Home/Error");
        app.UseHsts();
    }

    // 3. Swagger UI (disponible en todos los entornos)
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "ComprobantePago API v1");
        c.RoutePrefix = "swagger";
    });

    app.UseHttpsRedirection();
    app.UseStaticFiles();
    app.UseRouting();
    app.UseAuthentication();
    app.UseAuthorization();

    app.MapControllerRoute(
        name: "default",
        pattern: "{controller=Comprobante}/{action=Index}/{id?}");

    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "La aplicación terminó inesperadamente.");
}
finally
{
    Log.CloseAndFlush();
}
