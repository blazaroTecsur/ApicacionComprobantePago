using ComprobantePago.Application.DTOs.Seguridad;

namespace ComprobantePago.Application.Interfaces.Services
{
    public interface ISeguridadService
    {
        Task<IEnumerable<SeguridadRolResponse>> ObtenerPermisos(
            string codTenant, string codUsuario, string codApp);
    }
}
