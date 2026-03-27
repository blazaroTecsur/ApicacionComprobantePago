namespace ComprobantePago.Application.Exceptions
{
    public class ImputacionNotFoundException : Exception
    {
        public ImputacionNotFoundException(string folio, int secuencia)
            : base($"No se encontró la imputación {secuencia} para el folio '{folio}'.")
        { }
    }
}
