using ComprobantePago.Application.Commands.Comprobante;
using ComprobantePago.Application.Commands.Imputacion;
using ComprobantePago.Application.DTOs.Comprobante.Requests;
using ComprobantePago.Application.Interfaces.QueryServices;
using ComprobantePago.Application.Interfaces.Repositories;
using ComprobantePago.Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace ComprobantePago.Web.Controllers
{
    public class ComprobanteController : Controller
    {
        private readonly IComprobanteQueryService _queryService;
        private readonly IComprobanteRepository _repository;
        private readonly IExcelSytelineService _excelService;

        public ComprobanteController(
            IComprobanteQueryService queryService,
            IComprobanteRepository repository,
            IExcelSytelineService excelService)
        {
            _queryService = queryService;
            _repository = repository;
            _excelService = excelService;
        }

        // ══════════════════════════════════════
        // VISTAS
        // ══════════════════════════════════════

        // GET: /Comprobante/Index
        public IActionResult Index()
        {
            return View();
        }

        // GET: /Comprobante/Detalle
        public IActionResult Detalle()
        {
            return View();
        }

        #region COMBOS

        [HttpGet]
        public async Task<IActionResult> ObtenerTiposDocumento()
        {
            var data = await _queryService.ObtenerTiposDocumentoAsync();
            return Ok(data);
        }

        [HttpGet]
        public async Task<IActionResult> ObtenerTiposSunat()
        {
            var data = await _queryService.ObtenerTiposSunatAsync();
            return Ok(data);
        }

        [HttpGet]
        public async Task<IActionResult> ObtenerMonedas()
        {
            var data = await _queryService.ObtenerMonedasAsync();
            return Ok(data);
        }

        [HttpGet]
        public async Task<IActionResult> ObtenerLugaresPago()
        {
            var data = await _queryService.ObtenerLugaresPagoAsync();
            return Ok(data);
        }

        [HttpGet]
        public async Task<IActionResult> ObtenerTiposDetraccion()
        {
            var data = await _queryService.ObtenerTiposDetraccionAsync();
            return Ok(data);
        }

        [HttpGet]
        public async Task<IActionResult> ObtenerTipos()
        {
            var data = await _queryService.ObtenerTiposDocumentoAsync();
            return Ok(data);
        }

        [HttpGet]
        public async Task<IActionResult> ObtenerEstados()
        {
            var data = await _queryService.ObtenerEstadosAsync();
            return Ok(data);
        }

        #endregion

        // ══════════════════════════════════════
        // CONSULTAS COMPROBANTE
        // ══════════════════════════════════════

        [HttpPost]
        public async Task<IActionResult> Buscar(
            [FromBody] BuscarComprobanteDto filtros)
        {
            var data = await _queryService.BuscarAsync(filtros);
            return Ok(data);
        }

        [HttpGet]
        public async Task<IActionResult> ObtenerDetalle(string folio)
        {
            var data = await _queryService.ObtenerDetalleAsync(folio);
            if (data == null)
                return Ok(new
                {
                    exito = false,
                    mensaje = "Comprobante no encontrado."
                });
            return Ok(data);
        }

        [HttpGet]
        public async Task<IActionResult> ObtenerPdf(string folio)
        {
            var pdf = await _queryService.ObtenerPdfAsync(folio);
            if (pdf == null) return NotFound();
            return File(pdf, "application/pdf");
        }

        [HttpGet]
        public async Task<IActionResult> DocumentosElectronicos(string folio)
        {
            var data = await _queryService
                .ObtenerDocumentosElectronicosAsync(folio);
            return Ok(data);
        }

        // ══════════════════════════════════════
        // COMANDOS COMPROBANTE
        // ══════════════════════════════════════

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Guardar(
            [FromBody] RegistrarComprobanteCommand command)
        {
            try
            {
                var folio = await _repository.GuardarAsync(command);
                return Ok(new { exito = true, folio });
            }
            catch (Exception ex)
            {
                return Ok(new { exito = false, mensaje = ex.Message });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Enviar(
            [FromBody] EnviarComprobanteCommand command)
        {
            try
            {
                await _repository.EnviarAsync(command);
                return Ok(new { exito = true });
            }
            catch (Exception ex)
            {
                return Ok(new { exito = false, mensaje = ex.Message });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Firmar(
            [FromBody] FirmarComprobanteCommand command)
        {
            try
            {
                await _repository.FirmarAsync(command);
                return Ok(new { exito = true });
            }
            catch (Exception ex)
            {
                return Ok(new { exito = false, mensaje = ex.Message });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Aprobar(
            [FromBody] AprobarComprobanteCommand command)
        {
            try
            {
                await _repository.AprobarAsync(command);
                return Ok(new { exito = true });
            }
            catch (Exception ex)
            {
                return Ok(new { exito = false, mensaje = ex.Message });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Anular(
            [FromBody] AnularComprobanteCommand command)
        {
            try
            {
                await _repository.AnularAsync(command);
                return Ok(new { exito = true });
            }
            catch (Exception ex)
            {
                return Ok(new { exito = false, mensaje = ex.Message });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Derivar(
            [FromBody] DerivarComprobanteCommand command)
        {
            try
            {
                await _repository.DerivarAsync(command);
                return Ok(new { exito = true });
            }
            catch (Exception ex)
            {
                return Ok(new { exito = false, mensaje = ex.Message });
            }
        }

        // ══════════════════════════════════════
        // CONSULTAS IMPUTACIÓN
        // ══════════════════════════════════════

        [HttpGet]
        public async Task<IActionResult> ObtenerImputaciones(string folio)
        {
            var data = await _queryService.ObtenerImputacionesAsync(folio);
            return Ok(data);
        }

        [HttpGet]
        public async Task<IActionResult> ObtenerCodigosUnidad(
            string campo, int unidad, string codigo)
        {
            var data = await _queryService
                .ObtenerCodigosUnidadAsync(campo, unidad, codigo);
            return Ok(data);
        }

        [HttpGet]
        public async Task<IActionResult> DescargarPlantillaImputacion()
        {
            var archivo = await _queryService
                .ObtenerPlantillaImputacionAsync();
            return File(
                archivo,
                "application/vnd.openxmlformats-officedocument" +
                ".spreadsheetml.sheet",
                "PlantillaImputacion.xlsx"
            );
        }

        // ══════════════════════════════════════
        // COMANDOS IMPUTACIÓN
        // ══════════════════════════════════════

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AgregarImputacion(
            [FromBody] AgregarImputacionCommand command)
        {
            try
            {
                var imputacion = await _repository
                    .AgregarImputacionAsync(command);
                return Ok(new { exito = true, imputacion });
            }
            catch (Exception ex)
            {
                return Ok(new { exito = false, mensaje = ex.Message });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditarImputacion(
            [FromBody] EditarImputacionCommand command)
        {
            try
            {
                var imputacion = await _repository
                    .EditarImputacionAsync(command);
                return Ok(new { exito = true, imputacion });
            }
            catch (Exception ex)
            {
                return Ok(new { exito = false, mensaje = ex.Message });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EliminarImputacion(
            [FromBody] EliminarImputacionCommand command)
        {
            try
            {
                await _repository.EliminarImputacionAsync(command);
                return Ok(new { exito = true });
            }
            catch (Exception ex)
            {
                return Ok(new { exito = false, mensaje = ex.Message });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CargarImputacionMasiva(
            IFormFile file)
        {
            try
            {
                if (file == null || file.Length == 0)
                    return Ok(new
                    {
                        exito = false,
                        mensaje = "Archivo inválido."
                    });

                var imputaciones = await _repository
                    .CargarImputacionMasivaAsync(file);
                return Ok(new { exito = true, imputaciones });
            }
            catch (Exception ex)
            {
                return Ok(new { exito = false, mensaje = ex.Message });
            }
        }

        // POST: /Comprobante/ValidarXmlSunat
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ValidarXmlSunat(IFormFile archivo)
        {
            try
            {
                if (archivo == null || archivo.Length == 0)
                    return Ok(new
                    {
                        exito = false,
                        mensaje = "Archivo inválido."
                    });

                var resultado = await _repository.ValidarXmlSunatAsync(archivo);
                return Ok(resultado);
            }
            catch (Exception ex)
            {
                return Ok(new { exito = false, mensaje = ex.Message });
            }
        }

        // POST: /Comprobante/ValidarPdfSunat
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ValidarPdfSunat(IFormFile archivo)
        {
            try
            {
                if (archivo == null || archivo.Length == 0)
                    return Ok(new
                    {
                        exito = false,
                        mensaje = "Archivo inválido."
                    });

                var resultado = await _repository.ValidarPdfSunatAsync(archivo);
                return Ok(resultado);
            }
            catch (Exception ex)
            {
                return Ok(new { exito = false, mensaje = ex.Message });
            }
        }

        // POST: /Comprobante/ValidarZipSunat
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ValidarZipSunat(IFormFile archivo)
        {
            try
            {
                if (archivo == null || archivo.Length == 0)
                    return Ok(new
                    {
                        exito = false,
                        mensaje = "Archivo inválido."
                    });

                var resultado = await _repository.ValidarZipSunatAsync(archivo);
                return Ok(resultado);
            }
            catch (Exception ex)
            {
                return Ok(new { exito = false, mensaje = ex.Message });
            }
        }

        // POST: /Comprobante/SubirDocumentos
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SubirDocumentos(
            string folio, string subTipo, IFormFileCollection archivos)
        {
            try
            {
                if (string.IsNullOrEmpty(folio))
                    return Ok(new { exito = false, mensaje = "Folio requerido." });
                if (string.IsNullOrEmpty(subTipo))
                    return Ok(new { exito = false, mensaje = "Tipo de documento requerido." });
                if (archivos == null || archivos.Count == 0)
                    return Ok(new { exito = false, mensaje = "No se recibieron archivos." });

                var lista = new List<(byte[] contenido, string nombre, string tipo, string subTipo)>();
                foreach (var archivo in archivos)
                {
                    if (archivo.Length == 0) continue;
                    var ext = Path.GetExtension(archivo.FileName)
                        .TrimStart('.').ToLowerInvariant();
                    var tipo = ext == "pdf" ? "PDF" : ext.ToUpper();

                    using var ms = new MemoryStream();
                    await archivo.CopyToAsync(ms);
                    lista.Add((ms.ToArray(), archivo.FileName, tipo, subTipo));
                }

                if (lista.Count > 0)
                    await _repository.GuardarDocumentosAsync(folio, lista);

                var docs = await _queryService.ObtenerDocumentosElectronicosAsync(folio);
                return Ok(new { exito = true, documentos = docs });
            }
            catch (Exception ex)
            {
                return Ok(new { exito = false, mensaje = ex.Message });
            }
        }

        // GET: /Comprobante/DescargarDocumento?id={id}
        [HttpGet]
        public async Task<IActionResult> DescargarDocumento(int id)
        {
            var doc = await _repository.DescargarDocumentoAsync(id);
            if (doc == null) return NotFound();

            var ext = Path.GetExtension(doc.NombreArchivo)
                .TrimStart('.').ToLowerInvariant();
            var contentType = ext == "pdf" ? "application/pdf" : "application/octet-stream";

            return File(doc.Contenido, contentType, doc.NombreArchivo);
        }

        // POST: /Comprobante/EliminarDocumento
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EliminarDocumento([FromBody] int id)
        {
            try
            {
                await _repository.EliminarDocumentoAsync(id);
                return Ok(new { exito = true });
            }
            catch (Exception ex)
            {
                return Ok(new { exito = false, mensaje = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> ExportarDistribucionSyteline(
            [FromForm] List<string> folios)
        {
            if (folios == null || !folios.Any())
                return Ok(new
                {
                    exito = false,
                    mensaje = "No hay comprobantes seleccionados."
                });

            var datos = await _queryService
                .ObtenerDistribucionSytelineAsync(folios);

            if (!datos.Any())
                return Ok(new
                {
                    exito = false,
                    mensaje = "No hay imputaciones para exportar."
                });

            var excel = _excelService.GenerarDistribucion(datos);
            var fecha = DateTime.Now.ToString("yyyyMMdd_HHmmss");

            return File(
                excel,
                "application/vnd.openxmlformats-officedocument" +
                ".spreadsheetml.sheet",
                $"Distribucion_Syteline_{fecha}.xlsx"
            );
        }

        [HttpGet]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ExportarCabeceraSyteline(
            [FromBody] ExportarSytelineCommand command)
        {
            if (command?.Folios == null || !command.Folios.Any())
                return Ok(new
                {
                    exito = false,
                    mensaje = "No hay comprobantes seleccionados."
                });

            var datos = await _queryService
                .ObtenerCabecerasSytelineAsync(command.Folios);

            if (!datos.Any())
                return Ok(new
                {
                    exito = false,
                    mensaje = "No hay comprobantes aprobados para exportar."
                });

            var excel = _excelService.GenerarCabecera(datos);
            var fecha = DateTime.Now.ToString("yyyyMMdd_HHmmss");

            return File(
                excel,
                "application/vnd.openxmlformats-officedocument" +
                ".spreadsheetml.sheet",
                $"Cabecera_Syteline_{fecha}.xlsx"
            );
        }
    }
}
