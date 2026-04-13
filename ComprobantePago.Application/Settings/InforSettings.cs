namespace ComprobantePago.Application.Settings
{
    /// <summary>
    /// Configuración para la API IDO REST de Infor CloudSuite / Syteline.
    /// Los valores se mapean directamente desde los campos del archivo .ionapi.
    /// </summary>
    public class InforSettings
    {
        public const string Section = "Infor";

        /// <summary>URL base del portal ION API — campo pu del .ionapi cuando apunta a mingle-ionapi</summary>
        public string BaseUrl { get; set; } = string.Empty;

        /// <summary>
        /// URL base del SSO de Infor incluyendo tenant y /as/ — campo pu del .ionapi.
        /// Ej: https://mingle-sso.inforcloudsuite.com:443/EPXQLSZBRDZWPDCZ_TST/as/
        /// Se concatena con "token.oauth2" para obtener el TokenEndpoint.
        /// </summary>
        public string SsoBaseUrl { get; set; } = string.Empty;

        /// <summary>Tenant ID de Infor (ej. EPXQLSZBRDZWPDCZ_TST)</summary>
        public string Tenant { get; set; } = string.Empty;

        /// <summary>Identificador lógico de la aplicación Syteline (ej. CSI)</summary>
        public string AppId { get; set; } = string.Empty;

        /// <summary>Client ID — campo ci del .ionapi</summary>
        public string ClientId { get; set; } = string.Empty;

        /// <summary>Client Secret — campo cs del .ionapi</summary>
        public string ClientSecret { get; set; } = string.Empty;

        /// <summary>Usuario de servicio — campo dt del .ionapi</summary>
        public string Username { get; set; } = string.Empty;

        /// <summary>Service Account API Key — campo saak del .ionapi</summary>
        public string Password { get; set; } = string.Empty;

        // ── URLs derivadas ────────────────────────────────────────────────────

        /// <summary>
        /// Endpoint OAuth2 para obtener tokens: SsoBaseUrl + "token.oauth2"
        /// Equivale a pu + ot del .ionapi (pu ya incluye /TENANT/as/).
        /// </summary>
        public string TokenEndpoint =>
            $"{SsoBaseUrl.TrimEnd('/')}token.oauth2";

        /// <summary>URL base del servicio IDO REST</summary>
        public string IdoBaseUrl =>
            $"{BaseUrl.TrimEnd('/')}/{Tenant}/{AppId}/IDORequestService/MGRestService.svc/json/";
    }
}
