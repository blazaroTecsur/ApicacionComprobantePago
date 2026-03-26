namespace ComprobantePago.Application.DTOs.Comprobante.Response
{
    public class ComprobanteDto
    {
        public string Folio { get; set; }
        public string TipoComprobante { get; set; }
        public string Serie { get; set; }
        public string Numero { get; set; }
        public string Proveedor { get; set; }
        public string Fecha { get; set; }
        public string Moneda { get; set; }
        public decimal MontoTotal { get; set; }
        public string Estado { get; set; }
    }
}
