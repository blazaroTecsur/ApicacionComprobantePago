namespace ComprobantePago.Application.DTOs.Responses
{
    public class SytelineDistribucionDto
    {
        // ── Cabecera (repetida en las 3 subfilas) ───
        public string Proveedor { get; set; } = string.Empty;
        public int Comprobante { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string FechaDistribucion { get; set; } = string.Empty;
        public string Factura { get; set; } = string.Empty;
        public string FechaFactura { get; set; } = string.Empty;
        public decimal TasaCambio { get; set; }
        public string Moneda { get; set; } = string.Empty;
        public decimal ImpoCompra { get; set; }
        public decimal IGV { get; set; }
        public decimal MntoFactura { get; set; }
        public decimal TotalDistribucion { get; set; }
        public string NroProveedor { get; set; } = string.Empty;
        public string NombreProv { get; set; } = string.Empty;
        public string NumRegFiscal { get; set; } = string.Empty;

        // ── Por subfila ──────────────────────────────
        public int SecDist { get; set; }
        public string Proyecto { get; set; } = string.Empty;
        public string SistImpst { get; set; } = string.Empty;   // "", "1", "2"
        public string CodImp { get; set; } = string.Empty;      // "", "NR", "IGV18"
        public string DescCodImp { get; set; } = string.Empty;
        public decimal BaseImp { get; set; }
        public decimal Importe { get; set; }
        public string CuentaContable { get; set; } = string.Empty;
        public string DescripcionCuenta { get; set; } = string.Empty;
        public string CodUnidad1 { get; set; } = string.Empty;
        public string CodUnidad3 { get; set; } = string.Empty;
        public string CodUnidad4 { get; set; } = string.Empty;
        public bool EsLineaPrincipal { get; set; }
        public string TipoDoc { get; set; } = string.Empty;
    }
}
