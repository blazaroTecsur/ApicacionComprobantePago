namespace ComprobantePago.Application.Settings
{
    /// <summary>
    /// Configuración para la API IDO REST de Infor CloudSuite / Syteline.
    /// Los nombres de propiedad corresponden directamente a los campos del archivo .ionapi.
    /// </summary>
    public class InforSettings
    {
        public const string Section = "Infor";

        /// <summary>URL base del portal ION API — campo iu del .ionapi (mingle-ionapi)</summary>
        public string BaseUrl { get; set; } = string.Empty;

        /// <summary>
        /// URL base del SSO incluyendo tenant y /as/ — campo pu del .ionapi.
        /// Ej: https://mingle-sso.inforcloudsuite.com:443/EPXQLSZBRDZWPDCZ_TST/as/
        /// </summary>
        public string SsoBaseUrl { get; set; } = string.Empty;

        /// <summary>Tenant ID — campo ti del .ionapi</summary>
        public string Tenant { get; set; } = string.Empty;

        /// <summary>Identificador lógico de la aplicación Syteline (ej. CSI)</summary>
        public string AppId { get; set; } = string.Empty;

        /// <summary>OAuth2 Client ID — campo ci del .ionapi</summary>
        public string ClientId { get; set; } = string.Empty;

        /// <summary>OAuth2 Client Secret — campo cs del .ionapi</summary>
        public string ClientSecret { get; set; } = string.Empty;

        /// <summary>
        /// Service Account API Key — campo saak del .ionapi.
        /// Se usa como username en el flujo OAuth2 password grant.
        /// </summary>
        public string ServiceAccountKey { get; set; } = string.Empty;

        /// <summary>
        /// Service Account Secret Key — campo sask del .ionapi.
        /// Se usa como password en el flujo OAuth2 password grant.
        /// </summary>
        public string ServiceAccountSecret { get; set; } = string.Empty;

        // ── URLs derivadas ────────────────────────────────────────────────────

        /// <summary>Token endpoint: SsoBaseUrl + "token.oauth2" (equivale a pu + ot del .ionapi)</summary>
        public string TokenEndpoint =>
            $"{SsoBaseUrl.TrimEnd('/')}token.oauth2";

        /// <summary>URL base del servicio IDO REST</summary>
        public string IdoBaseUrl =>
            $"{BaseUrl.TrimEnd('/')}/{Tenant}/{AppId}/IDORequestService/MGRestService.svc/json/";
    }
}
