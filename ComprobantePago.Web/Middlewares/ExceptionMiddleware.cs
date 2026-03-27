using ComprobantePago.Application.Exceptions;
using FluentValidation;
using System.Text.Json;

namespace ComprobantePago.Web.Middlewares
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;
        private readonly IHostEnvironment _env;

        public ExceptionMiddleware(
            RequestDelegate next,
            ILogger<ExceptionMiddleware> logger,
            IHostEnvironment env)
        {
            _next = next;
            _logger = logger;
            _env = env;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,
                    "Error no manejado en {Method} {Path}: {Message}",
                    context.Request.Method,
                    context.Request.Path,
                    ex.Message);

                await ManejarExcepcionAsync(context, ex);
            }
        }

        private async Task ManejarExcepcionAsync(HttpContext context, Exception ex)
        {
            var (statusCode, mensaje) = ex switch
            {
                ComprobanteNotFoundException  => (StatusCodes.Status404NotFound, ex.Message),
                ImputacionNotFoundException   => (StatusCodes.Status404NotFound, ex.Message),
                ValidationException ve        => (StatusCodes.Status400BadRequest,
                    string.Join(" | ", ve.Errors.Select(e => e.ErrorMessage))),
                ArgumentException             => (StatusCodes.Status400BadRequest, ex.Message),
                UnauthorizedAccessException   => (StatusCodes.Status401Unauthorized, "No autorizado."),
                _                            => (StatusCodes.Status500InternalServerError,
                    _env.IsDevelopment() ? ex.ToString() : "Error interno del servidor.")
            };

            // Solicitudes AJAX / API → responder JSON
            if (EsAjax(context.Request))
            {
                context.Response.StatusCode = statusCode;
                context.Response.ContentType = "application/json";

                var body = JsonSerializer.Serialize(new
                {
                    exito   = false,
                    mensaje,
                    status  = statusCode
                });

                await context.Response.WriteAsync(body);
                return;
            }

            // Navegación MVC normal → redirigir a página de error
            context.Response.Redirect("/Home/Error");
        }

        private static bool EsAjax(HttpRequest request) =>
            request.Headers.XRequestedWith == "XMLHttpRequest" ||
            (request.ContentType?.Contains("application/json") ?? false) ||
            request.Path.StartsWithSegments("/api");
    }
}
