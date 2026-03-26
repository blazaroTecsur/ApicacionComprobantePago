using ComprobantePago.Application.DTOs.Comprobante.Requests;

namespace ComprobantePago.Application.Commands.Comprobante
{
    public class DerivarComprobanteCommand
    {
        public AccionComprobanteDto Comprobante { get; set; } = new();
    }
}
