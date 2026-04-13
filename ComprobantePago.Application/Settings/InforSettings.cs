namespace ComprobantePago.Application.Settings
{
    /// <summary>
    /// Configuración para la API IDO REST de Infor CloudSuite / Syteline.
    /// Mapea la sección "Infor" de appsettings.json.
    /// </summary>
    public class InforSettings
    {
        public const string Section = "Infor";

        /// <summary>URL raíz del portal ION: https://mingle-ionapi.inforcloudsuite.com</summary>
        public string BaseUrl { get; set; } = string.Empty;

        /// <summary>Tenant ID de Infor (ej. EPXQLSZBRDZWPDCZ_TST)</summary>
        public string Tenant { get; set; } = string.Empty;

        /// <summary>Identificador lógico de la aplicación Syteline (ej. CSI)</summary>
        public string AppId { get; set; } = string.Empty;

        /// <summary>Client ID de la aplicación registrada en Infor OS (campo ci del .ionapi)</summary>
        public string ClientId { get; set; } = string.Empty;

        /// <summary>Client Secret (campo cs del .ionapi)</summary>
        public string ClientSecret { get; set; } = string.Empty;

        /// <summary>Usuario de servicio de Infor OS (campo dt del .ionapi)</summary>
        public string Username { get; set; } = string.Empty;

        /// <summary>Contraseña del usuario de servicio</summary>
        public string Password { get; set; } = string.Empty;

        // ── URLs derivadas ────────────────────────────────────────────────────

        /// <summary>Endpoint OAuth2 para obtener tokens (field ot del .ionapi)</summary>
        public string TokenEndpoint =>
            $"{BaseUrl.TrimEnd('/')}/{Tenant}/as/token.oauth2";

        /// <summary>URL base del servicio IDO REST</summary>
        public string IdoBaseUrl =>
            $"{BaseUrl.TrimEnd('/')}/{Tenant}/{AppId}/IDORequestService/MGRestService.svc/json/";
    }
}
