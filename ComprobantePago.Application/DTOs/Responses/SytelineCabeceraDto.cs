namespace ComprobantePago.Application.DTOs.Responses
{
    public class SytelineCabeceraDto
    {
        // Campos del comprobante
        public string Proveedor { get; set; } = string.Empty;
        public string Nombre { get; set; } = string.Empty;
        public int Comprobante { get; set; }
        public string Factura { get; set; } = string.Empty;
        public string FechaFactura { get; set; } = string.Empty;
        public string FechaDistribucion { get; set; } = string.Empty;
        public decimal ImpoCompra { get; set; }
        public decimal CargosVarios { get; set; }
        public decimal ImpVentas2 { get; set; }
        public decimal MntoFactura { get; set; }
        public decimal ImpSinDesc { get; set; }
        public string FechaDcto { get; set; } = string.Empty;
        public int DiasVto { get; set; }
        public string FechaVen { get; set; } = string.Empty;
        public string Moneda { get; set; } = string.Empty;
        public decimal TipoCambio { get; set; }
        public string CtaCP { get; set; } = string.Empty;
        public string CtaCPUnid1 { get; set; } = string.Empty;
        public string DescripcionCuenta { get; set; } = string.Empty;
        public string Ref { get; set; } = string.Empty;
        public string EstadoAut { get; set; } = string.Empty;
        public string Autorizo { get; set; } = string.Empty;
        public string Notas { get; set; } = string.Empty;
        public string UsaDetraccion { get; set; } = string.Empty;
        public string Detraccion { get; set; } = string.Empty;
        public decimal Tasa { get; set; }
        public decimal TotalDetraccion { get; set; }
        public decimal TotalDetLocal { get; set; }
        public decimal MontoExento { get; set; }
        public decimal MontoRetencion { get; set; }
    }
}
