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
    /// Cliente para el servicio IDO REST de Infor Syteline (MGRestService.svc).
    /// URLs corregidas según la especificación real del servicio.
    /// </summary>
    public sealed class SytelineIdoService : ISytelineIdoService
    {
        private readonly HttpClient                  _http;
        private readonly IInforTokenService          _tokenService;
        private readonly InforSettings               _settings;
        private readonly ILogger<SytelineIdoService> _logger;

        private static readonly JsonSerializerOptions _jsonOpts = new()
        {
            PropertyNameCaseInsensitive = true
        };

        public SytelineIdoService(
            HttpClient                   http,
            IInforTokenService           tokenService,
            IOptions<InforSettings>      settings,
            ILogger<SytelineIdoService>  logger)
        {
            _http         = http;
            _tokenService = tokenService;
            _settings     = settings.Value;
            _logger       = logger;
        }

        // ── GET /json/{ido} — LoadCollection ─────────────────────────────────

        public async Task<JsonElement> LoadAsync(
            string  ido,
            string? props     = null,
            string? filter    = null,
            int     recordCap = 0,
            string? orderBy   = null,
            CancellationToken ct = default)
        {
            var url = $"{_settings.IdoBaseUrl}json/{Uri.EscapeDataString(ido)}";

            var q = new List<string>();
            if (!string.IsNullOrWhiteSpace(props))     q.Add($"props={Uri.EscapeDataString(props)}");
            if (!string.IsNullOrWhiteSpace(filter))    q.Add($"filter={Uri.EscapeDataString(filter)}");
            if (recordCap > 0)                         q.Add($"recordCap={recordCap}");
            if (!string.IsNullOrWhiteSpace(orderBy))   q.Add($"orderBy={Uri.EscapeDataString(orderBy)}");
            if (q.Count > 0) url += "?" + string.Join("&", q);

            _logger.LogDebug("IDO Load → {Url}", url);
            return await EjecutarGetAsync(url, ct);
        }

        // ── GET /json/idoinfo/{ido} — IDOPropInfo ────────────────────────────

        public async Task<JsonElement> IdoInfoAsync(
            string ido,
            CancellationToken ct = default)
        {
            var url = $"{_settings.IdoBaseUrl}json/idoinfo/{Uri.EscapeDataString(ido)}";
            _logger.LogDebug("IDO Info → {Url}", url);
            return await EjecutarGetAsync(url, ct);
        }

        // ── GET /json/method/{ido}/{method} — InvokeMethod ───────────────────

        public async Task<JsonElement> InvokeMethodAsync(
            string ido,
            string method,
            CancellationToken ct = default)
        {
            var url = $"{_settings.IdoBaseUrl}json/method/{Uri.EscapeDataString(ido)}/{Uri.EscapeDataString(method)}";
            _logger.LogDebug("IDO Method → {Url}", url);
            return await EjecutarGetAsync(url, ct);
        }

        // ── POST /json/{ido}/additem — InsertItem ────────────────────────────

        public async Task<JsonElement> InsertItemAsync(
            string ido,
            object payload,
            CancellationToken ct = default)
        {
            var url = $"{_settings.IdoBaseUrl}json/{Uri.EscapeDataString(ido)}/additem";
            _logger.LogDebug("IDO InsertItem → {Url}", url);
            return await EjecutarPostAsync(url, payload, ct);
        }

        // ── POST /json/{ido}/additems — InsertItems ──────────────────────────

        public async Task<JsonElement> InsertItemsAsync(
            string ido,
            object payload,
            CancellationToken ct = default)
        {
            var url = $"{_settings.IdoBaseUrl}json/{Uri.EscapeDataString(ido)}/additems";
            _logger.LogDebug("IDO InsertItems → {Url}", url);
            return await EjecutarPostAsync(url, payload, ct);
        }

        // ── PUT /json/{ido}/updateitem — UpdateItem ──────────────────────────

        public async Task<JsonElement> UpdateItemAsync(
            string ido,
            object payload,
            CancellationToken ct = default)
        {
            var url = $"{_settings.IdoBaseUrl}json/{Uri.EscapeDataString(ido)}/updateitem";
            _logger.LogDebug("IDO UpdateItem → {Url}", url);
            return await EjecutarPutAsync(url, payload, ct);
        }

        // ── GET /json/configurations ──────────────────────────────────────────

        public async Task<JsonElement> ObtenerConfiguracionesAsync(CancellationToken ct = default)
        {
            var url = $"{_settings.IdoBaseUrl}json/configurations";
            _logger.LogDebug("IDO Configurations → {Url}", url);
            return await EjecutarGetAsync(url, ct);
        }

        // ── Helpers privados ──────────────────────────────────────────────────

        private async Task<JsonElement> EjecutarGetAsync(string url, CancellationToken ct)
        {
            using var request = new HttpRequestMessage(HttpMethod.Get, url);
            await AgregarAuthHeaderAsync(request);
            using var respuesta = await _http.SendAsync(request, ct);
            return await LeerRespuestaAsync(respuesta, ct);
        }

        private async Task<JsonElement> EjecutarPostAsync(string url, object? cuerpo, CancellationToken ct)
        {
            using var request = new HttpRequestMessage(HttpMethod.Post, url)
            {
                Content = new StringContent(
                    JsonSerializer.Serialize(cuerpo, _jsonOpts),
                    Encoding.UTF8, "application/json")
            };
            await AgregarAuthHeaderAsync(request);
            using var respuesta = await _http.SendAsync(request, ct);
            return await LeerRespuestaAsync(respuesta, ct);
        }

        private async Task<JsonElement> EjecutarPutAsync(string url, object? cuerpo, CancellationToken ct)
        {
            using var request = new HttpRequestMessage(HttpMethod.Put, url)
            {
                Content = new StringContent(
                    JsonSerializer.Serialize(cuerpo, _jsonOpts),
                    Encoding.UTF8, "application/json")
            };
            await AgregarAuthHeaderAsync(request);
            using var respuesta = await _http.SendAsync(request, ct);
            return await LeerRespuestaAsync(respuesta, ct);
        }

        private async Task AgregarAuthHeaderAsync(HttpRequestMessage request)
        {
            var token = await _tokenService.ObtenerTokenAsync();
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

            // Header obligatorio de Mongoose: identifica la configuración/BD de Syteline.
            // Sin él el IDO responde MessageCode 302 "Missing Mongoose configuration header".
            if (!string.IsNullOrWhiteSpace(_settings.Configuration))
                request.Headers.TryAddWithoutValidation("X-Infor-MongooseConfig", _settings.Configuration);
        }

        private async Task<JsonElement> LeerRespuestaAsync(
            HttpResponseMessage respuesta, CancellationToken ct)
        {
            var contenido = await respuesta.Content.ReadAsStringAsync(ct);

            if (!respuesta.IsSuccessStatusCode)
            {
                _logger.LogError("Error IDO REST. Status: {Status} — {Body}",
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
