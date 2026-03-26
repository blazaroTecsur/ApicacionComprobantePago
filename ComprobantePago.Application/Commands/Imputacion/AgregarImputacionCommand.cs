using ComprobantePago.Application.DTOs.Comprobante.Requests;

namespace ComprobantePago.Application.Commands.Imputacion
{
    public class AgregarImputacionCommand
    {
        public ImputacionDto Imputacion { get; set; } = new();
    }
}
