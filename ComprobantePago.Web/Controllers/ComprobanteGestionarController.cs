using ComprobantePago.Application.Commands.Comprobante;
using ComprobantePago.Application.Commands.Imputacion;
using ComprobantePago.Application.Common;
using ComprobantePago.Application.DTOs.Comprobante.Requests;
using ComprobantePago.Application.Interfaces.QueryServices;
using ComprobantePago.Application.Interfaces.Repositories;
using ComprobantePago.Web.Authorization;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ComprobantePago.Web.Controllers
{
    [Route("Comprobante")]
    [Authorize]
    public class ComprobanteGestionarController : Controller
    {
        private readonly IComprobanteRepository              _repository;
        private readonly IComprobanteQueryService            _queryService;
        private readonly IValidator<RegistrarComprobanteDto> _comprobanteValidator;
        private readonly IValidator<ImputacionDto>           _imputacionValidator;
        private readonly IValidator<AccionComprobanteDto>    _accionValidator;

        public ComprobanteGestionarController(
            IComprobanteRepository              repository,
            IComprobanteQueryService            queryService,
            IValidator<RegistrarComprobanteDto> comprobanteValidator,
            IValidator<ImputacionDto>           imputacionValidator,
            IValidator<AccionComprobanteDto>    accionValidator)
        {
            _repository           = repository;
            _queryService         = queryService;
            _comprobanteValidator = comprobanteValidator;
            _imputacionValidator  = imputacionValidator;
            _accionValidator      = accionValidator;
        }

        // ══════════════════════════════════════
        // COMANDOS COMPROBANTE
        // ══════════════════════════════════════

        [HttpPost("[action]")]
        [ValidateAntiForgeryToken]
        [Permission("COMP.GUARDAR")]
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

        [HttpPost("[action]")]
        [ValidateAntiForgeryToken]
        [Permission("COMP.ENVIAR")]
        public async Task<IActionResult> Enviar([FromBody] EnviarComprobanteCommand command)
        {
            var validacion = await _accionValidator.ValidateAsync(command.Comprobante);
            if (!validacion.IsValid)
                return BadRequest(new { errores = validacion.Errors.Select(e => e.ErrorMessage) });
            await _repository.EnviarAsync(command);
            return Ok(BaseResponse.Ok());
        }

        [HttpPost("[action]")]
        [ValidateAntiForgeryToken]
        [Permission("COMP.ANULAR")]
        public async Task<IActionResult> Anular([FromBody] AnularComprobanteCommand command)
        {
            var validacion = await _accionValidator.ValidateAsync(command.Comprobante);
            if (!validacion.IsValid)
                return BadRequest(new { errores = validacion.Errors.Select(e => e.ErrorMessage) });
            await _repository.AnularAsync(command);
            return Ok(BaseResponse.Ok());
        }

        // ══════════════════════════════════════
        // COMANDOS IMPUTACIÓN
        // ══════════════════════════════════════

        [HttpPost("[action]")]
        [ValidateAntiForgeryToken]
        [Permission("COMP.GUARDAR")]
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

        [HttpPost("[action]")]
        [ValidateAntiForgeryToken]
        [Permission("COMP.GUARDAR")]
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

        [HttpPost("[action]")]
        [ValidateAntiForgeryToken]
        [Permission("COMP.GUARDAR")]
        public async Task<IActionResult> EliminarImputacion([FromBody] EliminarImputacionCommand command)
        {
            await _repository.EliminarImputacionAsync(command);
            return Ok(BaseResponse.Ok());
        }

        [HttpPost("[action]")]
        [ValidateAntiForgeryToken]
        [Permission("COMP.GUARDAR")]
        public async Task<IActionResult> CargarImputacionMasiva(IFormFile file)
        {
            var imputaciones = await _repository.CargarImputacionMasivaAsync(file);
            return Ok(new { exito = true, imputaciones });
        }

        // ══════════════════════════════════════
        // VALIDACIÓN Y GESTIÓN DE DOCUMENTOS
        // ══════════════════════════════════════

        [HttpPost("[action]")]
        [ValidateAntiForgeryToken]
        [Permission("COMP.GUARDAR")]
        public async Task<IActionResult> ValidarXmlSunat(IFormFile archivo)
        {
            var error = ValidarArchivo(archivo, 10, "xml");
            if (error != null) return BadRequest(error);
            return Ok(await _repository.ValidarXmlSunatAsync(archivo));
        }

        [HttpPost("[action]")]
        [ValidateAntiForgeryToken]
        [Permission("COMP.GUARDAR")]
        public async Task<IActionResult> ValidarPdfSunat(IFormFile archivo)
        {
            var error = ValidarArchivo(archivo, 20, "pdf");
            if (error != null) return BadRequest(error);
            return Ok(await _repository.ValidarPdfSunatAsync(archivo));
        }

        [HttpPost("[action]")]
        [ValidateAntiForgeryToken]
        [Permission("COMP.GUARDAR")]
        public async Task<IActionResult> ValidarZipSunat(IFormFile archivo)
        {
            var error = ValidarArchivo(archivo, 20, "zip");
            if (error != null) return BadRequest(error);
            return Ok(await _repository.ValidarZipSunatAsync(archivo));
        }

        [HttpPost("[action]")]
        [ValidateAntiForgeryToken]
        [Permission("COMP.GUARDAR")]
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

        [HttpGet("[action]")]
        [Permission("COMP.GUARDAR")]
        public async Task<IActionResult> DescargarDocumento([FromQuery] int id)
        {
            var doc = await _repository.DescargarDocumentoAsync(id);
            if (doc is null) return NotFound();
            var ext = Path.GetExtension(doc.NombreArchivo).TrimStart('.').ToLowerInvariant();
            var contentType = ext == "pdf" ? "application/pdf" : "application/octet-stream";
            return File(doc.Contenido, contentType, doc.NombreArchivo);
        }

        [HttpPost("[action]")]
        [ValidateAntiForgeryToken]
        [Permission("COMP.GUARDAR")]
        public async Task<IActionResult> EliminarDocumento([FromBody] int id)
        {
            await _repository.EliminarDocumentoAsync(id);
            return Ok(BaseResponse.Ok());
        }

        // ══════════════════════════════════════
        // HELPER
        // ══════════════════════════════════════

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
    }
}
