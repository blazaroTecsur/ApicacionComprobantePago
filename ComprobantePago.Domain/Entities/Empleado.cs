namespace ComprobantePago.Domain.Entities
{
    // SQL to create table:
    // CREATE TABLE rcoempleado (
    //   IdEmpleado INT NOT NULL AUTO_INCREMENT PRIMARY KEY,
    //   Codigo VARCHAR(20) NOT NULL,
    //   Nombre VARCHAR(200) NOT NULL,
    //   Activo TINYINT(1) NOT NULL DEFAULT 1
    // );
    public class Empleado
    {
        public int IdEmpleado { get; set; }
        public string Codigo { get; set; } = string.Empty;
        public string Nombre { get; set; } = string.Empty;
        public bool Activo { get; set; } = true;
    }
}
