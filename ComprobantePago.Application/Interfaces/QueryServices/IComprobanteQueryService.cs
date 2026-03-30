using ComprobantePago.Application.DTOs.Comprobante.Common;
using ComprobantePago.Application.DTOs.Comprobante.Requests;
using ComprobantePago.Application.DTOs.Comprobante.Response;
using ComprobantePago.Application.DTOs.Responses;


namespace ComprobantePago.Application.Interfaces.QueryServices
{
    public interface IComprobanteQueryService
    {
        // ── Combos ────────────────────────────────
        Task<IEnumerable<ComboDto>> ObtenerTiposDocumentoAsync();
        Task<IEnumerable<ComboDto>> ObtenerTiposSunatAsync();
        Task<IEnumerable<ComboDto>> ObtenerMonedasAsync();
        Task<IEnumerable<ComboDto>> ObtenerLugaresPagoAsync();
        Task<IEnumerable<ComboDto>> ObtenerTiposDetraccionAsync();
        Task<IEnumerable<ComboDto>> ObtenerEstadosAsync();
        Task<IEnumerable<ComboDto>> ObtenerEmpleadosAsync(string filtro = "");

        // ── Consultas Comprobante ─────────────────
        Task<IEnumerable<ComprobanteDto>> BuscarAsync(
            BuscarComprobanteDto filtros);
        Task<ComprobanteDetalleDto> ObtenerDetalleAsync(string folio);
        Task<byte[]> ObtenerPdfAsync(string folio);
        Task<IEnumerable<DocumentoElectronicoDto>> ObtenerDocumentosElectronicosAsync(
            string folio);

        // ── Consultas Imputación ──────────────────
        Task<IEnumerable<ImputacionDetalleDto>> ObtenerImputacionesAsync(
            string folio);
        Task<IEnumerable<ComboDto>> ObtenerCuentasContablesAsync(string filtro = "");
        Task<IEnumerable<ComboDto>> ObtenerCodigosUnidadAsync(
            string campo, int unidad, string codigo, string filtro = "");
        Task<byte[]> ObtenerPlantillaImputacionAsync();
        Task<IEnumerable<SytelineCabeceraDto>> ObtenerCabecerasSytelineAsync(List<string>? folios = null);
        Task<IEnumerable<SytelineDistribucionDto>> ObtenerDistribucionSytelineAsync(List<string>? folios = null);
    }
}
