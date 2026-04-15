using ComprobantePago.Application.DTOs.Infor;
using ComprobantePago.Application.DTOs.Responses;
using ComprobantePago.Application.Interfaces.Services;
using ComprobantePago.Application.Settings;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace ComprobantePago.Infrastructure.Services
{
    /// <summary>
    /// Envía comprobantes aprobados al IDO SLAptrxs de Infor Syteline.
    /// </summary>
    public sealed class SytelineEnvioService : ISytelineEnvioService
    {
        private readonly ISytelineIdoService             _ido;
        private readonly InforSettings                   _settings;
        private readonly ILogger<SytelineEnvioService>  _logger;

        public SytelineEnvioService(
            ISytelineIdoService            ido,
            IOptions<InforSettings>        settings,
            ILogger<SytelineEnvioService>  logger)
        {
            _ido      = ido;
            _settings = settings.Value;
            _logger   = logger;
        }

        // ── Insertar una cabecera ─────────────────────────────────────────────

        public async Task<int> EnviarCabeceraAsync(
            SytelineCabeceraDto cabecera,
            CancellationToken ct = default)
        {
            var dto     = MapearCabecera(cabecera);
            var payload = new { propertyList = dto };

            if (string.IsNullOrWhiteSpace(dto.VendNum) || dto.VendNum == "0")
                throw new InvalidOperationException(
                    $"El proveedor del comprobante '{dto.InvNum}' no tiene VendNum " +
                    $"asignado en el maestro de proveedores (IdProveedorExternal = 0 o vacío). " +
                    $"Sincronice el maestro de proveedores con Syteline antes de enviar.");

            _logger.LogInformation(
                "Enviando comprobante {InvNum} de proveedor {VendNum} a SLAptrxs...",
                dto.InvNum, dto.VendNum);

            var respuesta = await _ido.InsertItemAsync("SLAptrxs", payload, ct);

            var voucher = ExtraerVoucher(respuesta);

            _logger.LogInformation(
                "Comprobante {InvNum} insertado en SLAptrxs. Voucher: {Voucher}",
                dto.InvNum, voucher);

            return voucher;
        }

        // ── Insertar múltiples cabeceras ──────────────────────────────────────

        public async Task<IEnumerable<int>> EnviarCabecerasAsync(
            IEnumerable<SytelineCabeceraDto> cabeceras,
            CancellationToken ct = default)
        {
            var vouchers = new List<int>();

            foreach (var cabecera in cabeceras)
            {
                ct.ThrowIfCancellationRequested();
                var voucher = await EnviarCabeceraAsync(cabecera, ct);
                vouchers.Add(voucher);
            }

            return vouchers;
        }

        // ── Mapeo SytelineCabeceraDto → SLAptrxsInsertDto ────────────────────

        private SLAptrxsInsertDto MapearCabecera(SytelineCabeceraDto c) => new()
        {
            // VendNum viene de tmaproveedor.IdProveedorExternal (sincronizado desde SLVendors)
            VendNum   = c.VendNum,
            InvDate   = c.FechaFactura,
            DistDate  = c.FechaDistribucion,
            UbToSite  = _settings.Site,

            // Cabecera
            InvNum      = c.Factura[..Math.Min(22, c.Factura.Length)],
            PurchAmt    = c.ImpoCompra,
            SalesTax    = c.ImpVentas2,
            InvAmt      = c.MntoFactura,
            MiscCharges = c.CargosVarios,
            NonDiscAmt  = c.ImpSinDesc,

            // Vencimiento
            DueDate  = c.FechaVen,
            DueDays  = c.DiasVto,
            DiscDate = c.FechaDcto,

            // Moneda
            AptCurrCode = c.Moneda,
            ExchRate    = c.TipoCambio,

            // Cuenta A/P
            ApAcct      = c.CtaCP[..Math.Min(12, c.CtaCP.Length)],
            ApAcctUnit1 = c.CtaCPUnid1[..Math.Min(4, c.CtaCPUnid1.Length)],
            ApAcctUnit3 = c.CtaCPUnid3[..Math.Min(4, c.CtaCPUnid3.Length)],
            ApAcctUnit4 = c.CtaCPUnid4[..Math.Min(4, c.CtaCPUnid4.Length)],

            // Referencia
            Ref        = c.Ref[..Math.Min(30, c.Ref.Length)],
            Txt        = c.Notas[..Math.Min(40, c.Notas.Length)],
            Authorizer = c.Autorizo[..Math.Min(128, c.Autorizo.Length)],

            // Impuesto: Exento cuando no hay IGV o hay monto exento; NR en caso contrario
            TaxCode1   = (c.ImpVentas2 == 0 || c.MontoExento > 0) ? "Exento" : "NR",

            // Estado de autorización: siempre "F" (pendiente de autorización en Syteline)
            AuthStatus = "F",

            // Folio de origen
            aptZLA_SeqFac = c.Comprobante.ToString(),

            // Detracción
            aptZLA_UsaDetraccion        = c.UsaDetraccion == "1" ? (byte)1 : (byte)0,
            aptZLA_CodigoDetraccion     = c.Detraccion,
            aptZLA_TasaDetraccion       = c.Tasa,
            aptZLA_TotalDetraccion      = c.TotalDetraccion,
            aptZLA_TotalDetraccionLocal = c.TotalDetLocal,
        };

        // ── Extraer Voucher de la respuesta ───────────────────────────────────

        private static int ExtraerVoucher(JsonElement respuesta)
        {
            // Infor devuelve el item insertado con sus propiedades actualizadas
            if (respuesta.TryGetProperty("propertyList", out var props) &&
                props.TryGetProperty("Voucher", out var voucher))
            {
                return voucher.TryGetInt32(out var v) ? v : 0;
            }

            return 0;
        }
    }
}
