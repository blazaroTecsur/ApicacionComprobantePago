using ComprobantePago.Application.Interfaces.Services;
using ComprobantePago.Application.Settings;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace ComprobantePago.Infrastructure.Services
{
    /// <summary>
    /// Obtiene y cachea el token OAuth2 de Infor ION usando el flujo
    /// Resource Owner Password Credentials (cuenta de servicio).
    /// </summary>
    public sealed class InforTokenService : IInforTokenService
    {
        private readonly HttpClient      _http;
        private readonly InforSettings   _settings;
        private readonly ILogger<InforTokenService> _logger;

        // ── Caché en memoria ─────────────────────────────────────────────────
        private string   _tokenCache   = string.Empty;
        private DateTime _tokenExpira  = DateTime.MinValue;
        private readonly SemaphoreSlim _lock = new(1, 1);

        public InforTokenService(
            HttpClient                  http,
            IOptions<InforSettings>     settings,
            ILogger<InforTokenService>  logger)
        {
            _http     = http;
            _settings = settings.Value;
            _logger   = logger;
        }

        public async Task<string> ObtenerTokenAsync()
        {
            // Fast path: el token en caché sigue vigente
            if (!string.IsNullOrEmpty(_tokenCache) && DateTime.UtcNow < _tokenExpira)
                return _tokenCache;

            await _lock.WaitAsync();
            try
            {
                // Double-check dentro del lock
                if (!string.IsNullOrEmpty(_tokenCache) && DateTime.UtcNow < _tokenExpira)
                    return _tokenCache;

                _logger.LogInformation("Solicitando nuevo token a Infor ION...");

                // saak = username (Service Account API Key)
                // sask = password (Service Account Secret Key)
                var parametros = new Dictionary<string, string>
                {
                    { "grant_type",    "password" },
                    { "username",      _settings.ServiceAccountKey },
                    { "password",      _settings.ServiceAccountSecret },
                    { "client_id",     _settings.ClientId },
                    { "client_secret", _settings.ClientSecret },
                    { "scope",         "openid" }
                };

                using var respuesta = await _http.PostAsync(
                    _settings.TokenEndpoint,
                    new FormUrlEncodedContent(parametros));

                if (!respuesta.IsSuccessStatusCode)
                {
                    var cuerpo = await respuesta.Content.ReadAsStringAsync();
                    _logger.LogError(
                        "Error al obtener token Infor ION. Status: {Status} — {Body}",
                        respuesta.StatusCode, cuerpo);
                    throw new InvalidOperationException(
                        $"No se pudo obtener el token de Infor ION: {respuesta.StatusCode}");
                }

                var json = await respuesta.Content.ReadAsStringAsync();
                var doc  = JsonDocument.Parse(json).RootElement;

                _tokenCache  = doc.GetProperty("access_token").GetString()
                               ?? throw new InvalidOperationException("La respuesta no contiene access_token.");

                // Restar 60 s al tiempo de expiración para renovar antes de que venza
                var expiresIn = doc.TryGetProperty("expires_in", out var exp)
                    ? exp.GetInt32()
                    : 3600;

                _tokenExpira = DateTime.UtcNow.AddSeconds(expiresIn - 60);

                _logger.LogInformation(
                    "Token Infor ION obtenido. Expira en {Segundos}s.", expiresIn - 60);

                return _tokenCache;
            }
            finally
            {
                _lock.Release();
            }
        }
    }
}
