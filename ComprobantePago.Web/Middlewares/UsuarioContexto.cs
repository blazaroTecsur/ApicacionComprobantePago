using ComprobantePago.Application.Interfaces;
using System.Security.Claims;

namespace ComprobantePago.Web.Middlewares
{
    /// <summary>
    /// Extrae los datos del usuario autenticado desde los claims del token OIDC de Azure Entra ID.
    /// Cuando no hay token (desarrollo local / endpoints anónimos) todos los campos retornan string.Empty.
    /// </summary>
    public sealed class UsuarioContexto : IUsuarioContexto
    {
        public string CodTenant  { get; }
        public string CodUsuario { get; }
        public string Correo     { get; }
        public string Titulo     { get; }

        public UsuarioContexto(IHttpContextAccessor contexto)
        {
            var usuario = contexto.HttpContext?.User;

            if (usuario is null)
            {
                CodTenant = CodUsuario = Correo = Titulo = string.Empty;
                return;
            }

            string ObtenerClaim(params string[] tipos) => tipos
                .Select(t => usuario.FindFirst(t)?.Value)
                .FirstOrDefault(v => !string.IsNullOrWhiteSpace(v)) ?? string.Empty;

            CodTenant  = ObtenerClaim(
                "tid",
                "http://schemas.microsoft.com/identity/claims/tenantid");

            CodUsuario = ObtenerClaim(
                "oid",
                "http://schemas.microsoft.com/identity/claims/objectidentifier");

            Correo = ObtenerClaim(
                "preferred_username",
                "upn",
                "email",
                ClaimTypes.Email,
                ClaimTypes.Upn)
                .IfEmpty(usuario.Identity?.Name ?? string.Empty);

            Titulo = ObtenerClaim("name", ClaimTypes.Name);
        }
    }

    file static class StringExtensions
    {
        public static string IfEmpty(this string value, string fallback)
            => string.IsNullOrWhiteSpace(value) ? fallback : value;
    }
}
