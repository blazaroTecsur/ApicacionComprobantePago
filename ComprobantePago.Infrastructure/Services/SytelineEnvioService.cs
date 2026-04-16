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
            var dto = MapearCabecera(cabecera);

            if (string.IsNullOrWhiteSpace(dto.VendNum.Trim()) || dto.VendNum.Trim() == "0")
                throw new InvalidOperationException(
                    $"El proveedor del comprobante '{dto.InvNum}' no tiene VendNum " +
                    $"asignado en el maestro de proveedores (IdProveedorExternal = 0 o vacío). " +
                    $"Sincronice el maestro de proveedores con Syteline antes de enviar.");

            var propList = ConstruirPropiedades(dto);

            _logger.LogInformation(
                "Enviando comprobante {InvNum} de proveedor {VendNum} a SLAptrxs...",
                dto.InvNum, dto.VendNum);

            var respuesta = await _ido.InsertItemAsync("SLAptrxs", propList, ct);

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
            // "V" para facturas y demás; vacío para NC/ND (07/08) — se omite al filtrar
            Type = c.TipoSunat is "07" or "08" ? "" : "V",

            // VendNum: longitud fija de 7 caracteres, rellenado con espacios a la izquierda
            VendNum   = c.VendNum.PadLeft(7),
            InvDate   = c.FechaFactura,
            DistDate  = c.FechaDistribucion,
            UbToSite  = _settings.Site,

            // Cabecera
            InvNum      = c.Factura[..Math.Min(22, c.Factura.Length)],
            PurchAmt    = c.ImpoCompra,
            SalesTax_2  = c.ImpVentas2,
            InvAmt      = c.MntoFactura,
            MiscCharges = c.CargosVarios,
            NonDiscAmt  = c.ImpSinDesc,

            // Vencimiento
            DueDate  = c.FechaVen,
            DueDays  = c.DiasVto,
            DiscDate = c.FechaDcto,
            DiscPct  = 0.000m,

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

            // Impuesto: EXE cuando no hay IGV o hay monto exento; NR en caso contrario
            TaxCode1   = (c.ImpVentas2 == 0 || c.MontoExento > 0) ? "EXE" : "NR",
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

        // ── Construir lista de IdoProperty ───────────────────────────────────
        // UbToSite y Voucher se envían con IsNull=true: Syteline usa ambos para
        // asignar el número de voucher de la secuencia del site automáticamente.
        // Los campos string vacíos se omiten.
        private static List<IdoProperty> ConstruirPropiedades(SLAptrxsInsertDto dto)
        {
            var lista = new List<IdoProperty>();

            foreach (var prop in typeof(SLAptrxsInsertDto).GetProperties())
            {
                var valor = prop.GetValue(dto);

                // UbToSite y Voucher: incluir siempre con IsNull=true para que
                // Syteline genere el voucher de la secuencia del site.
                if (prop.Name == nameof(SLAptrxsInsertDto.UbToSite) ||
                    prop.Name == nameof(SLAptrxsInsertDto.Voucher))
                {
                    lista.Add(new IdoProperty
                    {
                        IsNull = true,
                        Name   = prop.Name,
                        Value  = valor?.ToString() ?? string.Empty
                    });
                    continue;
                }

                // Omitir strings vacíos
                if (valor is string s && string.IsNullOrEmpty(s)) continue;
                if (valor is null) continue;

                lista.Add(new IdoProperty
                {
                    IsNull = false,
                    Name   = prop.Name,
                    Value  = FormatearValor(valor)
                });
            }

            return lista;
        }

        private static string FormatearValor(object? valor) => valor switch
        {
            null    => "",
            decimal d  => d.ToString(System.Globalization.CultureInfo.InvariantCulture),
            double  db => db.ToString(System.Globalization.CultureInfo.InvariantCulture),
            float   f  => f.ToString(System.Globalization.CultureInfo.InvariantCulture),
            _       => valor.ToString() ?? ""
        };

        // ── Extraer Voucher de la respuesta ───────────────────────────────────

        private static int ExtraerVoucher(JsonElement respuesta)
        {
            // Formato Action/Properties: el voucher viene en UpdatedItems[0].Properties
            if (respuesta.TryGetProperty("UpdatedItems", out var items) &&
                items.GetArrayLength() > 0)
            {
                var item = items[0];
                if (item.TryGetProperty("Properties", out var props) &&
                    props.ValueKind == JsonValueKind.Array)
                {
                    foreach (var prop in props.EnumerateArray())
                    {
                        if (prop.TryGetProperty("Name",  out var name)  &&
                            name.GetString() == "Voucher"                &&
                            prop.TryGetProperty("Value", out var value)  &&
                            int.TryParse(value.GetString(), out var v))
                        {
                            return v;
                        }
                    }
                }
            }

            return 0;
        }
    }
}
