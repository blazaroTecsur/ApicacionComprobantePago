using ComprobantePago.Application.DTOs.Comprobante.Requests;

namespace ComprobantePago.Application.Commands.Comprobante
{
    public class RegistrarComprobanteCommand
    {
        public RegistrarComprobanteDto Comprobante { get; set; } = new();
    }
}
