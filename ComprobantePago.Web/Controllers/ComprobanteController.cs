using ComprobantePago.Application.Commands.Comprobante;
using ComprobantePago.Application.Commands.Imputacion;
using ComprobantePago.Application.Common;
using ComprobantePago.Application.DTOs.Comprobante.Requests;
using ComprobantePago.Application.Interfaces.QueryServices;
using ComprobantePago.Application.Interfaces.Repositories;
using ComprobantePago.Application.Interfaces.Services;
using ComprobantePago.Application.Interfaces.Services.Maestros;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace ComprobantePago.Web.Controllers
{
    /// <summary>Gestión de comprobantes de pago.</summary>
    public class ComprobanteController : Controller
    {
        private readonly IComprobanteQueryService             _queryService;
        private readonly ISytelineQueryService               _sytelineService;
        private readonly IMaestrosQueryService               _maestrosService;
        private readonly IComprobanteRepository             _repository;
        private readonly IExcelSytelineService              _excelService;
        private readonly IProveedorService                  _proveedorService;
        private readonly IValidator<RegistrarComprobanteDto> _comprobanteValidator;
        private readonly IValidator<ImputacionDto>           _imputacionValidator;

        public ComprobanteController(
            IComprobanteQueryService             queryService,
            ISytelineQueryService               sytelineService,
            IMaestrosQueryService               maestrosService,
            IComprobanteRepository             repository,
            IExcelSytelineService              excelService,
            IProveedorService                  proveedorService,
            IValidator<RegistrarComprobanteDto> comprobanteValidator,
            IValidator<ImputacionDto>           imputacionValidator)
        {
            _queryService         = queryService;
            _sytelineService      = sytelineService;
            _maestrosService      = maestrosService;
            _repository           = repository;
            _excelService         = excelService;
            _proveedorService     = proveedorService;
            _comprobanteValidator = comprobanteValidator;
            _imputacionValidator  = imputacionValidator;
        }

        // ══════════════════════════════════════
        // VISTAS MVC  →  /Comprobante/Index | /Comprobante/Detalle
        // ══════════════════════════════════════

        public IActionResult Index() => View();

        public IActionResult Detalle() => View();

        // ══════════════════════════════════════
        // COMBOS  →  GET /Comprobante/{action}
        // ══════════════════════════════════════

        /// <summary>Devuelve los tipos de documento disponibles.</summary>
        [HttpGet]
        public async Task<IActionResult> ObtenerTiposDocumento()
            => Ok(await _maestrosService.ObtenerTiposDocumentoAsync());

        /// <summary>Devuelve los tipos SUNAT disponibles.</summary>
        [HttpGet]
        public async Task<IActionResult> ObtenerTiposSunat()
            => Ok(await _maestrosService.ObtenerTiposSunatAsync());

        /// <summary>Devuelve las monedas disponibles.</summary>
        [HttpGet]
        public async Task<IActionResult> ObtenerMonedas()
            => Ok(await _maestrosService.ObtenerMonedasAsync());

        /// <summary>Devuelve los lugares de pago disponibles.</summary>
        [HttpGet]
        public async Task<IActionResult> ObtenerLugaresPago()
            => Ok(await _maestrosService.ObtenerLugaresPagoAsync());

        /// <summary>Devuelve los tipos de detracción disponibles.</summary>
        [HttpGet]
        public async Task<IActionResult> ObtenerTiposDetraccion()
            => Ok(await _maestrosService.ObtenerTiposDetraccionAsync());

        /// <summary>Alias para ObtenerTiposDocumento.</summary>
        [HttpGet]
        public async Task<IActionResult> ObtenerTipos()
            => Ok(await _maestrosService.ObtenerTiposDocumentoAsync());

        /// <summary>Devuelve los estados de comprobante disponibles.</summary>
        [HttpGet]
        public async Task<IActionResult> ObtenerEstados()
            => Ok(await _maestrosService.ObtenerEstadosAsync());

        /// <summary>Busca empleados por filtro de código o nombre.</summary>
        [HttpGet]
        public async Task<IActionResult> ObtenerEmpleados([FromQuery] string filtro = "")
            => Ok(await _maestrosService.ObtenerEmpleadosAsync(filtro));

        /// <summary>Busca proveedores por filtro de RUC o razón social.</summary>
        [HttpGet]
        public async Task<IActionResult> ObtenerProveedores([FromQuery] string filtro = "")
            => Ok(await _proveedorService.ObtenerProveedoresAsync(filtro));

        // ══════════════════════════════════════
        // CONSULTAS COMPROBANTE
        // ══════════════════════════════════════

        /// <summary>Busca comprobantes aplicando los filtros indicados.</summary>
        [HttpPost]
        public async Task<IActionResult> Buscar([FromBody] BuscarComprobanteDto filtros)
            => Ok(await _queryService.BuscarAsync(filtros));

        /// <summary>Devuelve el detalle completo de un comprobante por folio.</summary>
        [HttpGet]
        public async Task<IActionResult> ObtenerDetalle([FromQuery] string folio)
        {
            var data = await _queryService.ObtenerDetalleAsync(folio);
            return data is null
                ? Ok(BaseResponse.Error("Comprobante no encontrado."))
                : Ok(data);
        }

        /// <summary>Genera y devuelve el PDF del comprobante (vista previa o descarga).</summary>
        [HttpGet]
        public async Task<IActionResult> ObtenerPdf([FromQuery] string folio, [FromQuery] bool descargar = false)
        {
            var pdf = await _queryService.ObtenerPdfAsync(folio);
            if (pdf is null) return NotFound();
            var nombre = $"Comprobante_{folio}.pdf";
            return descargar
                ? File(pdf, "application/pdf", nombre)
                : File(pdf, "application/pdf");
        }

        /// <summary>Lista los documentos electrónicos adjuntos a un comprobante.</summary>
        [HttpGet]
        public async Task<IActionResult> DocumentosElectronicos([FromQuery] string folio)
            => Ok(await _queryService.ObtenerDocumentosElectronicosAsync(folio));

        // ══════════════════════════════════════
        // COMANDOS COMPROBANTE
        // ══════════════════════════════════════

        /// <summary>Registra o actualiza un comprobante.</summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Guardar([FromBody] RegistrarComprobanteCommand command)
        {
            var validacion = await _comprobanteValidator.ValidateAsync(command.Comprobante);
            if (!validacion.IsValid)
                return BadRequest(new
                {
                    success = false,
                    error = new { code = "VALIDATION_ERROR", userMessage = validacion.Errors.First().ErrorMessage }
                });

            var folio = await _repository.GuardarAsync(command);
            return Ok(new { exito = true, folio });
        }

        /// <summary>Envía el comprobante al siguiente estado.</summary>x
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Enviar([FromBody] EnviarComprobanteCommand command)
        {
            await _repository.EnviarAsync(command);
            return Ok(BaseResponse.Ok());
        }

        /// <summary>Firma (autoriza) el comprobante.</summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Firmar([FromBody] FirmarComprobanteCommand command)
        {
            await _repository.FirmarAsync(command);
            return Ok(BaseResponse.Ok());
        }

        /// <summary>Aprueba el comprobante.</summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Aprobar([FromBody] AprobarComprobanteCommand command)
        {
            await _repository.AprobarAsync(command);
            return Ok(BaseResponse.Ok());
        }

        /// <summary>Anula el comprobante.</summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Anular([FromBody] AnularComprobanteCommand command)
        {
            await _repository.AnularAsync(command);
            return Ok(BaseResponse.Ok());
        }

        /// <summary>Deriva el comprobante.</summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Derivar([FromBody] DerivarComprobanteCommand command)
        {
            await _repository.DerivarAsync(command);
            return Ok(BaseResponse.Ok());
        }

        // ══════════════════════════════════════
        // CONSULTAS IMPUTACIÓN
        // ══════════════════════════════════════

        /// <summary>Lista las imputaciones contables de un comprobante.</summary>
        [HttpGet]
        public async Task<IActionResult> ObtenerImputaciones([FromQuery] string folio)
            => Ok(await _queryService.ObtenerImputacionesAsync(folio));

        /// <summary>Busca cuentas contables por filtro.</summary>
        [HttpGet]
        public async Task<IActionResult> ObtenerCuentasContables([FromQuery] string filtro = "")
            => Ok(await _maestrosService.ObtenerCuentasContablesAsync(filtro));

        /// <summary>Busca códigos de unidad por campo/unidad/código/filtro.</summary>
        [HttpGet]
        public async Task<IActionResult> ObtenerCodigosUnidad(
            string campo, int unidad, string codigo, string filtro = "")
            => Ok(await _maestrosService.ObtenerCodigosUnidadAsync(campo, unidad, codigo, filtro));

        /// <summary>Descarga la plantilla Excel para carga masiva de imputaciones.</summary>
        [HttpGet]
        public async Task<IActionResult> DescargarPlantillaImputacion()
        {
            var archivo = await _queryService.ObtenerPlantillaImputacionAsync();
            return File(archivo,
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                "PlantillaImputacion.xlsx");
        }

        // ══════════════════════════════════════
        // COMANDOS IMPUTACIÓN
        // ══════════════════════════════════════

        /// <summary>Agrega una línea de imputación contable.</summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AgregarImputacion([FromBody] AgregarImputacionCommand command)
        {
            var validacion = await _imputacionValidator.ValidateAsync(command.Imputacion);
            if (!validacion.IsValid)
                return BadRequest(new
                {
                    success = false,
                    error = new { code = "VALIDATION_ERROR", userMessage = validacion.Errors.First().ErrorMessage }
                });

            var imputacion = await _repository.AgregarImputacionAsync(command);
            return Ok(new { exito = true, imputacion });
        }

        /// <summary>Edita una línea de imputación contable existente.</summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditarImputacion([FromBody] EditarImputacionCommand command)
        {
            var validacion = await _imputacionValidator.ValidateAsync(command.Imputacion);
            if (!validacion.IsValid)
                return BadRequest(new
                {
                    success = false,
                    error = new { code = "VALIDATION_ERROR", userMessage = validacion.Errors.First().ErrorMessage }
                });

            var imputacion = await _repository.EditarImputacionAsync(command);
            return Ok(new { exito = true, imputacion });
        }

        /// <summary>Elimina una línea de imputación contable.</summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EliminarImputacion([FromBody] EliminarImputacionCommand command)
        {
            await _repository.EliminarImputacionAsync(command);
            return Ok(BaseResponse.Ok());
        }

        /// <summary>Carga masiva de imputaciones desde un archivo Excel.</summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CargarImputacionMasiva(IFormFile file)
        {
            var imputaciones = await _repository.CargarImputacionMasivaAsync(file);
            return Ok(new { exito = true, imputaciones });
        }

        // ══════════════════════════════════════
        // VALIDACIÓN SUNAT
        // ══════════════════════════════════════

        /// <summary>
        /// Valida que el archivo no sea nulo, no exceda el tamaño máximo y tenga
        /// una de las extensiones permitidas. Devuelve el objeto de error o null si es válido.
        /// </summary>
        private static object? ValidarArchivo(IFormFile? archivo, int maxMb, params string[] extensiones)
        {
            if (archivo is null || archivo.Length == 0)
                return new { success = false, error = new { code = "FILE_REQUIRED", userMessage = "Debe seleccionar un archivo." } };

            if (archivo.Length > maxMb * 1024 * 1024)
                return new { success = false, error = new { code = "FILE_TOO_LARGE", userMessage = $"El archivo no debe superar {maxMb} MB." } };

            var ext = Path.GetExtension(archivo.FileName).TrimStart('.').ToLowerInvariant();
            if (extensiones.Length > 0 && !extensiones.Contains(ext))
                return new { success = false, error = new { code = "FILE_TYPE_INVALID", userMessage = $"Tipo de archivo no permitido. Se acepta: {string.Join(", ", extensiones)}." } };

            return null;
        }

        /// <summary>Valida un comprobante electrónico mediante su archivo XML SUNAT.</summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ValidarXmlSunat(IFormFile archivo)
        {
            var error = ValidarArchivo(archivo, 10, "xml");
            if (error != null) return BadRequest(error);
            return Ok(await _repository.ValidarXmlSunatAsync(archivo));
        }

        /// <summary>Valida un comprobante electrónico mediante su archivo PDF.</summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ValidarPdfSunat(IFormFile archivo)
        {
            var error = ValidarArchivo(archivo, 20, "pdf");
            if (error != null) return BadRequest(error);
            return Ok(await _repository.ValidarPdfSunatAsync(archivo));
        }

        /// <summary>Valida un comprobante electrónico mediante un archivo ZIP (XML + CDR + PDF).</summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ValidarZipSunat(IFormFile archivo)
        {
            var error = ValidarArchivo(archivo, 20, "zip");
            if (error != null) return BadRequest(error);
            return Ok(await _repository.ValidarZipSunatAsync(archivo));
        }

        // ══════════════════════════════════════
        // GESTIÓN DOCUMENTOS ADJUNTOS
        // ══════════════════════════════════════

        /// <summary>Sube documentos adjuntos a un comprobante.</summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SubirDocumentos(
            string folio, string subTipo, IFormFileCollection archivos)
        {
            var extensionesPermitidas = new[] { "pdf", "xml", "jpg", "jpeg", "png", "xlsx", "xls" };
            var lista = new List<(byte[], string, string, string)>();
            foreach (var archivo in archivos.Where(a => a.Length > 0))
            {
                var error = ValidarArchivo(archivo, 20, extensionesPermitidas);
                if (error != null) return BadRequest(error);

                var ext  = Path.GetExtension(archivo.FileName).TrimStart('.').ToLowerInvariant();
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

        /// <summary>Descarga un documento adjunto por su ID.</summary>
        [HttpGet]
        public async Task<IActionResult> DescargarDocumento([FromQuery] int id)
        {
            var doc = await _repository.DescargarDocumentoAsync(id);
            if (doc is null) return NotFound();
            var ext = Path.GetExtension(doc.NombreArchivo).TrimStart('.').ToLowerInvariant();
            var contentType = ext == "pdf" ? "application/pdf" : "application/octet-stream";
            return File(doc.Contenido, contentType, doc.NombreArchivo);
        }

        /// <summary>Elimina un documento adjunto por su ID.</summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EliminarDocumento([FromBody] int id)
        {
            await _repository.EliminarDocumentoAsync(id);
            return Ok(BaseResponse.Ok());
        }

        // ══════════════════════════════════════
        // EXPORTACIÓN SYTELINE
        // ══════════════════════════════════════

        /// <summary>Exporta la distribución SyteLine en formato Excel para los folios indicados.</summary>
        [HttpPost]
        public async Task<IActionResult> ExportarDistribucionSyteline([FromForm] List<string> folios)
        {
            var datos = await _sytelineService.ObtenerDistribucionSytelineAsync(folios);
            var excel = _excelService.GenerarDistribucion(datos);
            return File(excel,
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                $"Distribucion_Syteline_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx");
        }

        /// <summary>Exporta la cabecera SyteLine en formato Excel para los folios indicados.</summary>
        [HttpPost]
        public async Task<IActionResult> ExportarCabeceraSyteline([FromForm] List<string> folios)
        {
            var datos = await _sytelineService.ObtenerCabecerasSytelineAsync(folios);
            var excel = _excelService.GenerarCabecera(datos);
            return File(excel,
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                $"Cabecera_Syteline_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx");
        }
    }
}
