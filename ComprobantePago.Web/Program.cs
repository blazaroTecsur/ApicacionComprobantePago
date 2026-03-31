using ComprobantePago.Application.Common;
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
using Microsoft.EntityFrameworkCore;
using Serilog;
using Serilog.Events;

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

    // ── Serilog como proveedor de logging ────────────────────────────────────
    builder.Host.UseSerilog();

    // ── MVC ──────────────────────────────────────────────────────────────────
    builder.Services.AddControllersWithViews()
        .AddJsonOptions(options =>
            options.JsonSerializerOptions.PropertyNameCaseInsensitive = true);

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

    // ── HttpClient SUNAT ──────────────────────────────────────────────────────
    builder.Services.AddHttpClient<ISunatService, SunatService>()
        .ConfigurePrimaryHttpMessageHandler(() =>
            new HttpClientHandler
            {
                UseProxy = false,
                ServerCertificateCustomValidationCallback =
                    HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
            }
        );

    // ── AutoMapper ────────────────────────────────────────────────────────────
    builder.Services.AddAutoMapper(typeof(ComprobanteMappingProfile).Assembly);

    // ── FluentValidation ──────────────────────────────────────────────────────
    builder.Services.AddValidatorsFromAssemblyContaining<RegistrarComprobanteValidator>();

    // ── Configuración ApiMaestros ─────────────────────────────────────────────
    builder.Services.Configure<ApiMaestrosSettings>(
        builder.Configuration.GetSection(ApiMaestrosSettings.Section));

    var usarApiMaestros = builder.Configuration
        .GetValue<bool>($"{ApiMaestrosSettings.Section}:UsarApi");

    if (usarApiMaestros)
    {
        // Fuente: API externa
        builder.Services.AddHttpClient<IEmpleadoService, ApiEmpleadoService>();
        builder.Services.AddHttpClient<IProveedorService, ApiProveedorService>();
        builder.Services.AddHttpClient<ICatalogoUnidadService, ApiCatalogoUnidadService>();
        builder.Services.AddHttpClient<ICuentaContableService, ApiCuentaContableService>();
    }
    else
    {
        // Fuente: base de datos local (tma* tables)
        builder.Services.AddScoped<IEmpleadoService, DbEmpleadoService>();
        builder.Services.AddScoped<IProveedorService, DbProveedorService>();
        builder.Services.AddScoped<ICatalogoUnidadService, DbCatalogoUnidadService>();
        builder.Services.AddScoped<ICuentaContableService, DbCuentaContableService>();
    }

    // ── Servicios de aplicación ───────────────────────────────────────────────
    builder.Services.AddScoped<XmlComprobanteService>();
    builder.Services.AddScoped<PdfComprobanteService>();
    builder.Services.AddScoped<IComprobanteQueryService, ComprobanteQueryService>();
    builder.Services.AddScoped<IComprobanteRepository, ComprobanteRepository>();
    builder.Services.AddScoped<IExcelSytelineService, ExcelSytelineService>();

    var app = builder.Build();

    // ── Pipeline de middlewares ───────────────────────────────────────────────
    // 1. Manejo global de excepciones (debe ir primero)
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

    app.UseHttpsRedirection();
    app.UseStaticFiles();
    app.UseRouting();
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
