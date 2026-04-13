using System.Text.Json;

namespace ComprobantePago.Application.Interfaces.Services
{
    /// <summary>
    /// Cliente para el servicio IDO REST de Infor Syteline
    /// (MGRestService.svc/json/).
    /// </summary>
    public interface ISytelineIdoService
    {
        /// <summary>
        /// Consulta registros de un IDO.
        /// </summary>
        /// <param name="ido">Nombre del IDO (ej. "SLVendors")</param>
        /// <param name="props">Propiedades separadas por coma (ej. "VendNum,Name")</param>
        /// <param name="filter">Filtro estilo SQL (ej. "VendNum = 'VENDOR01'")</param>
        /// <param name="recordCap">Límite de registros (0 = sin límite del servidor)</param>
        /// <param name="orderBy">Campo y dirección (ej. "VendNum ASC")</param>
        Task<JsonElement> LoadAsync(
            string ido,
            string? props     = null,
            string? filter    = null,
            int     recordCap = 0,
            string? orderBy   = null,
            CancellationToken ct = default);

        /// <summary>
        /// Invoca un método de un IDO.
        /// </summary>
        /// <param name="ido">Nombre del IDO</param>
        /// <param name="method">Nombre del método</param>
        /// <param name="parametros">Cuerpo JSON de parámetros</param>
        Task<JsonElement> InvokeAsync(
            string      ido,
            string      method,
            object?     parametros = null,
            CancellationToken ct   = default);

        /// <summary>
        /// Inserta o actualiza registros en un IDO.
        /// </summary>
        /// <param name="ido">Nombre del IDO</param>
        /// <param name="payload">Cuerpo JSON con los Items a actualizar</param>
        Task<JsonElement> UpdateAsync(
            string      ido,
            object      payload,
            CancellationToken ct = default);

        /// <summary>
        /// Lista las configuraciones de IDO disponibles en la instancia.
        /// Útil para descubrir los IDOs accesibles.
        /// </summary>
        Task<JsonElement> ObtenerConfiguracionesAsync(CancellationToken ct = default);
    }
}
