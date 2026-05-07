using ComprobantePago.Application.Commands.Comprobante;
using ComprobantePago.Application.Common;
using ComprobantePago.Application.DTOs.Comprobante.Requests;
using ComprobantePago.Application.Interfaces.Repositories;
using ComprobantePago.Web.Authorization;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace ComprobantePago.Web.Controllers
{
    [Route("Comprobante")]
    [Permission("COMP.AUTORIZAR")]
    public class ComprobanteAutorizarController : Controller
    {
        private readonly IComprobanteRepository          _repository;
        private readonly IValidator<AccionComprobanteDto> _accionValidator;

        public ComprobanteAutorizarController(
            IComprobanteRepository          repository,
            IValidator<AccionComprobanteDto> accionValidator)
        {
            _repository      = repository;
            _accionValidator = accionValidator;
        }

        [HttpPost("[action]")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Firmar([FromBody] FirmarComprobanteCommand command)
        {
            var validacion = await _accionValidator.ValidateAsync(command.Comprobante);
            if (!validacion.IsValid)
                return BadRequest(new { errores = validacion.Errors.Select(e => e.ErrorMessage) });
            await _repository.FirmarAsync(command);
            return Ok(BaseResponse.Ok());
        }

        [HttpPost("[action]")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Derivar([FromBody] DerivarComprobanteCommand command)
        {
            var validacion = await _accionValidator.ValidateAsync(command.Comprobante);
            if (!validacion.IsValid)
                return BadRequest(new { errores = validacion.Errors.Select(e => e.ErrorMessage) });
            await _repository.DerivarAsync(command);
            return Ok(BaseResponse.Ok());
        }
    }
}
