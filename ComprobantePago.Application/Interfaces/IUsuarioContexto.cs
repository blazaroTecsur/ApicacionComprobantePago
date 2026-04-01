namespace ComprobantePago.Application.Interfaces
{
    public interface IUsuarioContexto
    {
        /// <summary>ID del tenant Azure AD (claim "tid"). Identifica la empresa (GCI, Los Andes, Tecsur).</summary>
        string CodTenant { get; }

        /// <summary>ID de objeto del usuario en Azure AD (claim "oid"). Único por usuario dentro del tenant.</summary>
        string CodUsuario { get; }

        /// <summary>Correo / UPN del usuario autenticado.</summary>
        string Correo { get; }

        /// <summary>Nombre para mostrar del usuario (claim "name").</summary>
        string Titulo { get; }
    }
}
