namespace ComprobantePago.Application.DTOs.Comprobante.Requests
{
    public class ImputacionDto
    {
        public string Secuencia { get; set; }
        public string Folio { get; set; }
        public string AliasCuenta { get; set; }
        public string CuentaContable { get; set; }
        public string DescripcionCuenta { get; set; }
        public string Descripcion { get; set; }
        public decimal Monto { get; set; }
        public string Referencia { get; set; }
        public string Not { get; set; }

        // Auxiliares
        public string Fondo { get; set; }
        public string Dividendo { get; set; }
        public string Varios { get; set; }
        public string Actividad { get; set; }
        public string CentroResponsabilidad { get; set; }
        public string Proyecto { get; set; }
        public string CalidadRed { get; set; }
        public string UbicacionGeografica { get; set; }
        public string SubRecurso { get; set; }
        public string ActividadIngreso { get; set; }
        public string Cajero { get; set; }
        public string Proveedor { get; set; }
        public string JerarquiaCargo { get; set; }

        // Códigos Unidad Cuenta
        public string CodUnidad1Cuenta { get; set; }
        public string CodUnidad3Cuenta { get; set; }
        public string CodUnidad4Cuenta { get; set; }

        // Códigos Unidad Actividad
        public string CodUnidad1Actividad { get; set; }
        public string CodUnidad3Actividad { get; set; }
        public string CodUnidad4Actividad { get; set; }

        // Códigos Unidad SubRecurso
        public string CodUnidad1SubRecurso { get; set; }
        public string CodUnidad3SubRecurso { get; set; }
        public string CodUnidad4SubRecurso { get; set; }
    }
}
