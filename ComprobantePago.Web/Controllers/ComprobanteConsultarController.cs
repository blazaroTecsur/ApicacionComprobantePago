using ComprobantePago.Application.DTOs.Comprobante.Requests;
using ComprobantePago.Application.Interfaces.QueryServices;
using ComprobantePago.Application.Interfaces.Services;
using ComprobantePago.Application.Interfaces.Services.Maestros;
using ComprobantePago.Application.Common;
using ComprobantePago.Web.Authorization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ComprobantePago.Web.Controllers
{
    [Route("Comprobante")]
    [Permission("COMP.VER")]
    public class ComprobanteConsultarController : Controller
    {
        private readonly IComprobanteQueryService _queryService;
        private readonly IMaestrosQueryService    _maestrosService;
        private readonly IProveedorService        _proveedorService;

        public ComprobanteConsultarController(
            IComprobanteQueryService queryService,
            IMaestrosQueryService    maestrosService,
            IProveedorService        proveedorService)
        {
            _queryService     = queryService;
            _maestrosService  = maestrosService;
            _proveedorService = proveedorService;
        }

        // ══════════════════════════════════════
        // VISTAS MVC
        // ══════════════════════════════════════

        [AllowAnonymous]
        [HttpGet("[action]")]
        public IActionResult Index() => View("~/Views/Comprobante/Index.cshtml");

        [AllowAnonymous]
        [HttpGet("[action]")]
        public IActionResult Detalle() => View("~/Views/Comprobante/Detalle.cshtml");

        // ══════════════════════════════════════
        // COMBOS / CATÁLOGOS
        // ══════════════════════════════════════

        [HttpGet("[action]")]
        public async Task<IActionResult> ObtenerTiposDocumento()
            => Ok(await _maestrosService.ObtenerTiposDocumentoAsync());

        [HttpGet("[action]")]
        public async Task<IActionResult> ObtenerTiposSunat()
            => Ok(await _maestrosService.ObtenerTiposSunatAsync());

        [HttpGet("[action]")]
        public async Task<IActionResult> ObtenerMonedas()
            => Ok(await _maestrosService.ObtenerMonedasAsync());

        [HttpGet("[action]")]
        public async Task<IActionResult> ObtenerLugaresPago()
            => Ok(await _maestrosService.ObtenerLugaresPagoAsync());

        [HttpGet("[action]")]
        public async Task<IActionResult> ObtenerTiposDetraccion()
            => Ok(await _maestrosService.ObtenerTiposDetraccionAsync());

        [HttpGet("[action]")]
        public async Task<IActionResult> ObtenerTipos()
            => Ok(await _maestrosService.ObtenerTiposDocumentoAsync());

        [HttpGet("[action]")]
        public async Task<IActionResult> ObtenerEstados()
            => Ok(await _maestrosService.ObtenerEstadosAsync());

        [HttpGet("[action]")]
        public async Task<IActionResult> ObtenerEmpleados([FromQuery] string filtro = "")
            => Ok(await _maestrosService.ObtenerEmpleadosAsync(filtro));

        [HttpGet("[action]")]
        public async Task<IActionResult> ObtenerProveedores([FromQuery] string filtro = "")
            => Ok(await _proveedorService.ObtenerProveedoresAsync(filtro));

        // ══════════════════════════════════════
        // CONSULTAS COMPROBANTE
        // ══════════════════════════════════════

        [HttpPost("[action]")]
        public async Task<IActionResult> Buscar([FromBody] BuscarComprobanteDto filtros)
            => Ok(await _queryService.BuscarAsync(filtros));

        [HttpGet("[action]")]
        public async Task<IActionResult> ObtenerDetalle([FromQuery] string folio)
        {
            var data = await _queryService.ObtenerDetalleAsync(folio);
            return data is null
                ? Ok(BaseResponse.Error("Comprobante no encontrado."))
                : Ok(data);
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> ObtenerPdf([FromQuery] string folio, [FromQuery] bool descargar = false)
        {
            var pdf = await _queryService.ObtenerPdfAsync(folio);
            if (pdf is null) return NotFound();
            var nombre = $"Comprobante_{folio}.pdf";
            return descargar
                ? File(pdf, "application/pdf", nombre)
                : File(pdf, "application/pdf");
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> DocumentosElectronicos([FromQuery] string folio)
            => Ok(await _queryService.ObtenerDocumentosElectronicosAsync(folio));

        // ══════════════════════════════════════
        // CONSULTAS IMPUTACIÓN
        // ══════════════════════════════════════

        [HttpGet("[action]")]
        public async Task<IActionResult> ObtenerImputaciones([FromQuery] string folio)
            => Ok(await _queryService.ObtenerImputacionesAsync(folio));

        [HttpGet("[action]")]
        public async Task<IActionResult> ObtenerCuentasContables([FromQuery] string filtro = "")
            => Ok(await _maestrosService.ObtenerCuentasContablesAsync(filtro));

        [HttpGet("[action]")]
        public async Task<IActionResult> ObtenerCodigosUnidad(
            string campo, int unidad, string codigo, string filtro = "")
            => Ok(await _maestrosService.ObtenerCodigosUnidadAsync(campo, unidad, codigo, filtro));

        [HttpGet("[action]")]
        public async Task<IActionResult> DescargarPlantillaImputacion()
        {
            var archivo = await _queryService.ObtenerPlantillaImputacionAsync();
            return File(archivo,
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                "PlantillaImputacion.xlsx");
        }

    }
}
