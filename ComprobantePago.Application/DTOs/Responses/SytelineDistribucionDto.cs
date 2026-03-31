namespace ComprobantePago.Application.DTOs.Responses
{
    public sealed class SytelineDistribucionDto
    {
        // ── Cabecera (repetida en las 3 subfilas) ───
        public string Proveedor          { get; init; } = string.Empty;
        public int Comprobante           { get; init; }
        public string Nombre             { get; init; } = string.Empty;
        public string FechaDistribucion  { get; init; } = string.Empty;
        public string Factura            { get; init; } = string.Empty;
        public string FechaFactura       { get; init; } = string.Empty;
        public decimal TasaCambio        { get; init; }
        public string Moneda             { get; init; } = string.Empty;
        public decimal ImpoCompra        { get; init; }
        public decimal IGV               { get; init; }
        public decimal MntoFactura       { get; init; }
        public decimal TotalDistribucion { get; init; }
        public string NroProveedor       { get; init; } = string.Empty;
        public string NombreProv         { get; init; } = string.Empty;
        public string NumRegFiscal       { get; init; } = string.Empty;

        // ── Por subfila ──────────────────────────────
        public int SecDist               { get; init; }
        public string Proyecto           { get; init; } = string.Empty;
        public string SistImpst          { get; init; } = string.Empty;
        public string CodImp             { get; init; } = string.Empty;
        public string DescCodImp         { get; init; } = string.Empty;
        public decimal BaseImp           { get; init; }
        public decimal Importe           { get; init; }
        public string CuentaContable     { get; init; } = string.Empty;
        public string DescripcionCuenta  { get; init; } = string.Empty;
        public string CodUnidad1         { get; init; } = string.Empty;
        public string CodUnidad3         { get; init; } = string.Empty;
        public string CodUnidad4         { get; init; } = string.Empty;
        public bool EsLineaPrincipal     { get; init; }
        public string TipoDoc            { get; init; } = string.Empty;
    }
}
