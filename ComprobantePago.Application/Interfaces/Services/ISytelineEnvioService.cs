using ComprobantePago.Application.DTOs.Responses;

namespace ComprobantePago.Application.Interfaces.Services
{
    /// <summary>
    /// Envía comprobantes aprobados al IDO SLAptrxs de Infor Syteline.
    /// </summary>
    public interface ISytelineEnvioService
    {
        /// <summary>
        /// Inserta una cabecera de comprobante en SLAptrxs.
        /// Devuelve el número de voucher asignado por Syteline.
        /// </summary>
        Task<int> EnviarCabeceraAsync(
            SytelineCabeceraDto cabecera,
            CancellationToken ct = default);

        /// <summary>
        /// Inserta múltiples cabeceras en SLAptrxs en una sola operación.
        /// </summary>
        Task<IEnumerable<int>> EnviarCabecerasAsync(
            IEnumerable<SytelineCabeceraDto> cabeceras,
            CancellationToken ct = default);
    }
}
