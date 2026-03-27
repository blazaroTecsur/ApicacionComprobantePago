namespace ComprobantePago.Application.Exceptions
{
    public class ComprobanteNotFoundException : Exception
    {
        public ComprobanteNotFoundException(string folio)
            : base($"No se encontró el comprobante con folio '{folio}'.")
        { }
    }
}
