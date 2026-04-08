using ComprobantePago.Application.DTOs.Comprobante.Requests;
using FluentValidation;

namespace ComprobantePago.Application.Validations
{
    public class ImputacionValidator : AbstractValidator<ImputacionDto>
    {
        public ImputacionValidator()
        {
            RuleFor(x => x.Folio)
                .NotEmpty().WithMessage("El folio es obligatorio.");

            RuleFor(x => x.CuentaContable)
                .NotEmpty().WithMessage("La cuenta contable es obligatoria.");

            RuleFor(x => x.DescripcionCuenta)
                .NotEmpty().WithMessage("La descripción de cuenta es obligatoria.")
                .MaximumLength(200).WithMessage("La descripción no puede superar 200 caracteres.");

            RuleFor(x => x.CodUnidad1Cuenta)
                .NotEmpty().WithMessage("El código de unidad 1 es obligatorio.");

            RuleFor(x => x.CodUnidad3Cuenta)
                .NotEmpty().WithMessage("El código de unidad 3 es obligatorio.");

            RuleFor(x => x.CodUnidad4Cuenta)
                .NotEmpty().WithMessage("El código de unidad 4 es obligatorio.");
        }
    }
}
