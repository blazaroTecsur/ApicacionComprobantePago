using ComprobantePago.Application.Common;
using ComprobantePago.Application.Interfaces.QueryServices;
using ComprobantePago.Application.Interfaces.Repositories;
using ComprobantePago.Application.Interfaces.Services;
using ComprobantePago.Infrastructure.Persistence;
using ComprobantePago.Infrastructure.QueryServices;
using ComprobantePago.Infrastructure.Repositories;
using ComprobantePago.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using System.Net;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddControllersWithViews()
    .AddJsonOptions(options =>
    {
        // Permite recibir camelCase desde JS
        options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
    });

// ?? BD MySQL ??????????????????????????????????
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseMySql(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        ServerVersion.AutoDetect(
            builder.Configuration.GetConnectionString("DefaultConnection"))
    )
);

// Configuración SUNAT
builder.Services.Configure<SunatSettings>(
    builder.Configuration.GetSection("Sunat"));

// ?? HttpClient SUNAT con Proxy ????????????????
builder.Services.AddHttpClient<ISunatService, SunatService>()
    .ConfigurePrimaryHttpMessageHandler(() =>
        new HttpClientHandler
        {
            // Forzar conexión directa sin proxy
            UseProxy = false,
            ServerCertificateCustomValidationCallback =
                HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
        }
    );

// XML Service
builder.Services.AddScoped<XmlComprobanteService>();
// PDF Service
builder.Services.AddScoped<PdfComprobanteService>();

// Registrar dependencias
builder.Services.AddScoped<IComprobanteQueryService, ComprobanteQueryService>();
builder.Services.AddScoped<IComprobanteRepository, ComprobanteRepository>();
builder.Services.AddScoped<IExcelSytelineService, ExcelSytelineService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
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
