using ComprobantePago.Application.DTOs.Comprobante.Response;
using System.Xml.Linq;

namespace ComprobantePago.Infrastructure.Services
{
    public class XmlComprobanteService
    {
        // Namespaces del XML de SUNAT
        private static readonly XNamespace cbc =
            "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2";
        private static readonly XNamespace cac =
            "urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2";
        private static readonly XNamespace ext =
            "urn:oasis:names:specification:ubl:schema:xsd:CommonExtensionComponents-2";

        public DatosXmlDto LeerDatosXml(Stream stream)
        {
            var xml = XDocument.Load(stream);
            var root = xml.Root;

            var datos = new DatosXmlDto();

            try
            {
                // Serie y Número
                var id = root?.Element(cbc + "ID")?.Value ?? "";
                if (id.Contains("-"))
                {
                    datos.Serie = id.Split('-')[0];
                    datos.Numero = id.Split('-')[1];
                }

                // Fecha emisión
                var fechaRaw = root?
                    .Element(cbc + "IssueDate")?.Value ?? "";

                // Convertir de yyyy-MM-dd a dd/MM/yyyy
                if (DateTime.TryParse(fechaRaw, out var fecha))
                {
                    datos.FechaEmision = fecha.ToString("dd/MM/yyyy");
                }
                else
                {
                    datos.FechaEmision = fechaRaw;
                }

                // Tipo documento (código SUNAT)
                datos.TipoSunat = root?
                    .Element(cbc + "InvoiceTypeCode")?.Value ?? "";

                // Moneda
                datos.Moneda = root?
                    .Element(cbc + "DocumentCurrencyCode")?.Value ?? "";

                // También leer el proveedor (emisor) por separado
                var supplier = root?.Element(cac + "AccountingSupplierParty");
                var partySupplier = supplier?.Element(cac + "Party");

                datos.RucProveedor = partySupplier?
                    .Element(cac + "PartyIdentification")?
                    .Element(cbc + "ID")?.Value ?? string.Empty;

                datos.RazonSocialProveedor = partySupplier?
                    .Element(cac + "PartyLegalEntity")?
                    .Element(cbc + "RegistrationName")?.Value ?? string.Empty;

                // RUC y Razón Social del proveedor (emisor)
                var customer = root?
                    .Element(cac + "AccountingCustomerParty");
                var partyCustomer = customer?
                    .Element(cac + "Party");

                datos.Ruc = partyCustomer?
                    .Element(cac + "PartyIdentification")?
                    .Element(cbc + "ID")?.Value ?? "";

                datos.RazonSocial = partyCustomer?
                    .Element(cac + "PartyLegalEntity")?
                    .Element(cbc + "RegistrationName")?.Value ?? "";

                // Montos
                var monetaryTotal = root?
                    .Element(cac + "LegalMonetaryTotal");

                var totalStr = monetaryTotal?
                    .Element(cbc + "PayableAmount")?.Value ?? "0";
                datos.MontoTotal = decimal.TryParse(
                    totalStr,
                    System.Globalization.NumberStyles.Any,
                    System.Globalization.CultureInfo.InvariantCulture,
                    out var total) ? total : 0;

                var netoStr = monetaryTotal?
                    .Element(cbc + "LineExtensionAmount")?.Value ?? "0";
                datos.MontoNeto = decimal.TryParse(
                    netoStr,
                    System.Globalization.NumberStyles.Any,
                    System.Globalization.CultureInfo.InvariantCulture,
                    out var neto) ? neto : 0;

                // IGV
                var taxTotal = root?
                    .Element(cac + "TaxTotal");
                var igvStr = taxTotal?
                    .Element(cbc + "TaxAmount")?.Value ?? "0";
                datos.MontoIGV = decimal.TryParse(
                    igvStr,
                    System.Globalization.NumberStyles.Any,
                    System.Globalization.CultureInfo.InvariantCulture,
                    out var igv) ? igv : 0;

                // Detracción
                // El XML tiene dos <cac:PaymentTerms>:
                //   - ID = "Detraccion"  → tiene PaymentPercent y Amount
                //   - ID = "FormaPago"   → forma de pago, ignorar
                var paymentTerms = root?
                    .Elements(cac + "PaymentTerms")
                    .FirstOrDefault(pt =>
                        pt.Element(cbc + "ID")?.Value == "Detraccion");

                if (paymentTerms != null)
                {
                    datos.TieneDetraccion = true;

                    // Código de detracción (ej: "030" = Contratos de Construcción)
                    datos.CodigoDetraccion = paymentTerms
                        .Element(cbc + "PaymentMeansID")?.Value ?? string.Empty;

                    // Porcentaje (ej: "4.00")
                    var pctStr = paymentTerms
                        .Element(cbc + "PaymentPercent")?.Value ?? "0";
                    datos.PorcentajeDetraccion = decimal.TryParse(
                        pctStr,
                        System.Globalization.NumberStyles.Any,
                        System.Globalization.CultureInfo.InvariantCulture,
                        out var pct) ? pct : 0;

                    // Monto (ej: "466.00")
                    var montoDetStr = paymentTerms
                        .Element(cbc + "Amount")?.Value ?? "0";
                    datos.MontoDetraccion = decimal.TryParse(
                        montoDetStr,
                        System.Globalization.NumberStyles.Any,
                        System.Globalization.CultureInfo.InvariantCulture,
                        out var montoDet) ? montoDet : 0;
                }
            }
            catch
            {
                // Si falla la lectura devuelve lo que pudo leer
            }

            return datos;
        }
    }
}
