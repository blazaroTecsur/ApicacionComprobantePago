namespace ComprobantePago.Application.DTOs.Seguridad
{
    public class SeguridadResponse<T>
    {
        public bool Success { get; set; }
        public T Data { get; set; } = default!;
        public SeguridadErrorResponse? Error { get; set; }
    }
}
