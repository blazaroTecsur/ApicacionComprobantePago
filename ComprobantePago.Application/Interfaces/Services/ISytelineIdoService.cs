using System.Text.Json;

namespace ComprobantePago.Application.Interfaces.Services
{
    /// <summary>
    /// Cliente para el servicio IDO REST de Infor Syteline (MGRestService.svc).
    /// URLs según la especificación real del servicio.
    /// </summary>
    public interface ISytelineIdoService
    {
        /// <summary>
        /// Consulta registros de un IDO.
        /// GET /json/{ido}?props=...&amp;filter=...&amp;recordCap=N&amp;orderBy=...
        /// </summary>
        Task<JsonElement> LoadAsync(
            string ido,
            string? props     = null,
            string? filter    = null,
            int     recordCap = 0,
            string? orderBy   = null,
            CancellationToken ct = default);

        /// <summary>
        /// Devuelve la definición de propiedades de un IDO (schema).
        /// GET /json/idoinfo/{ido}
        /// </summary>
        Task<JsonElement> IdoInfoAsync(
            string ido,
            CancellationToken ct = default);

        /// <summary>
        /// Invoca un método de un IDO.
        /// GET /json/method/{ido}/{method}
        /// </summary>
        Task<JsonElement> InvokeMethodAsync(
            string ido,
            string method,
            CancellationToken ct = default);

        /// <summary>
        /// Inserta un registro en un IDO usando el formato Action/ItemId/Properties.
        /// POST /json/{ido}/additem
        /// </summary>
        Task<JsonElement> InsertItemAsync(
            string ido,
            IDictionary<string, object?> properties,
            CancellationToken ct = default);

        /// <summary>
        /// Inserta múltiples registros en un IDO.
        /// POST /json/{ido}/additems
        /// </summary>
        Task<JsonElement> InsertItemsAsync(
            string ido,
            object payload,
            CancellationToken ct = default);

        /// <summary>
        /// Actualiza un registro en un IDO.
        /// PUT /json/{ido}/updateitem
        /// </summary>
        Task<JsonElement> UpdateItemAsync(
            string ido,
            object payload,
            CancellationToken ct = default);

        /// <summary>
        /// Lista las configuraciones IDO disponibles.
        /// GET /json/configurations
        /// </summary>
        Task<JsonElement> ObtenerConfiguracionesAsync(CancellationToken ct = default);
    }
}
