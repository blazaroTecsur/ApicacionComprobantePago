using ComprobantePago.Application.Interfaces.Services;
using ComprobantePago.Application.Settings;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace ComprobantePago.Infrastructure.Services
{
    /// <summary>
    /// Cliente para el servicio IDO REST de Infor Syteline
    /// (MGRestService.svc/json/).
    /// Todas las llamadas incluyen automáticamente el Bearer token de Infor ION.
    /// </summary>
    public sealed class SytelineIdoService : ISytelineIdoService
    {
        private readonly HttpClient          _http;
        private readonly IInforTokenService  _tokenService;
        private readonly InforSettings       _settings;
        private readonly ILogger<SytelineIdoService> _logger;

        private static readonly JsonSerializerOptions _jsonOpts = new()
        {
            PropertyNameCaseInsensitive = true
        };

        public SytelineIdoService(
            HttpClient                      http,
            IInforTokenService              tokenService,
            IOptions<InforSettings>         settings,
            ILogger<SytelineIdoService>     logger)
        {
            _http         = http;
            _tokenService = tokenService;
            _settings     = settings.Value;
            _logger       = logger;
        }

        // ── Load ─────────────────────────────────────────────────────────────

        public async Task<JsonElement> LoadAsync(
            string  ido,
            string? props     = null,
            string? filter    = null,
            int     recordCap = 0,
            string? orderBy   = null,
            CancellationToken ct = default)
        {
            var url = $"{_settings.IdoBaseUrl}load/{Uri.EscapeDataString(ido)}";

            var queryParams = new List<string>();
            if (!string.IsNullOrWhiteSpace(props))
                queryParams.Add($"props={Uri.EscapeDataString(props)}");
            if (!string.IsNullOrWhiteSpace(filter))
                queryParams.Add($"filter={Uri.EscapeDataString(filter)}");
            if (recordCap > 0)
                queryParams.Add($"recordCap={recordCap}");
            if (!string.IsNullOrWhiteSpace(orderBy))
                queryParams.Add($"orderBy={Uri.EscapeDataString(orderBy)}");

            if (queryParams.Count > 0)
                url += "?" + string.Join("&", queryParams);

            _logger.LogDebug("IDO Load → {Url}", url);
            return await EjecutarGetAsync(url, ct);
        }

        // ── Invoke ───────────────────────────────────────────────────────────

        public async Task<JsonElement> InvokeAsync(
            string    ido,
            string    method,
            object?   parametros = null,
            CancellationToken ct = default)
        {
            var url = $"{_settings.IdoBaseUrl}invoke/{Uri.EscapeDataString(ido)}/{Uri.EscapeDataString(method)}";

            _logger.LogDebug("IDO Invoke → {Url}", url);
            return await EjecutarPostAsync(url, parametros, ct);
        }

        // ── Update ───────────────────────────────────────────────────────────

        public async Task<JsonElement> UpdateAsync(
            string    ido,
            object    payload,
            CancellationToken ct = default)
        {
            var url = $"{_settings.IdoBaseUrl}update/{Uri.EscapeDataString(ido)}";

            _logger.LogDebug("IDO Update → {Url}", url);
            return await EjecutarPostAsync(url, payload, ct);
        }

        // ── Configurations ───────────────────────────────────────────────────

        public async Task<JsonElement> ObtenerConfiguracionesAsync(CancellationToken ct = default)
        {
            var url = $"{_settings.IdoBaseUrl}configurations";
            _logger.LogDebug("IDO Configurations → {Url}", url);
            return await EjecutarGetAsync(url, ct);
        }

        // ── Helpers privados ─────────────────────────────────────────────────

        private async Task<JsonElement> EjecutarGetAsync(string url, CancellationToken ct)
        {
            using var request = new HttpRequestMessage(HttpMethod.Get, url);
            await AgregarAuthHeaderAsync(request);

            using var respuesta = await _http.SendAsync(request, ct);
            return await LeerRespuestaAsync(respuesta, ct);
        }

        private async Task<JsonElement> EjecutarPostAsync(
            string url, object? cuerpo, CancellationToken ct)
        {
            var json = cuerpo is null
                ? "{}"
                : JsonSerializer.Serialize(cuerpo, _jsonOpts);

            using var request = new HttpRequestMessage(HttpMethod.Post, url)
            {
                Content = new StringContent(json, Encoding.UTF8, "application/json")
            };
            await AgregarAuthHeaderAsync(request);

            using var respuesta = await _http.SendAsync(request, ct);
            return await LeerRespuestaAsync(respuesta, ct);
        }

        private async Task AgregarAuthHeaderAsync(HttpRequestMessage request)
        {
            var token = await _tokenService.ObtenerTokenAsync();
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }

        private async Task<JsonElement> LeerRespuestaAsync(
            HttpResponseMessage respuesta, CancellationToken ct)
        {
            var contenido = await respuesta.Content.ReadAsStringAsync(ct);

            if (!respuesta.IsSuccessStatusCode)
            {
                _logger.LogError(
                    "Error en IDO REST. Status: {Status} — {Body}",
                    respuesta.StatusCode, contenido);

                throw new InvalidOperationException(
                    $"Error en API Infor Syteline ({respuesta.StatusCode}): {contenido}");
            }

            _logger.LogDebug("IDO Response ({Status}): {Body}",
                (int)respuesta.StatusCode, contenido);

            return JsonDocument.Parse(contenido).RootElement.Clone();
        }
    }
}
