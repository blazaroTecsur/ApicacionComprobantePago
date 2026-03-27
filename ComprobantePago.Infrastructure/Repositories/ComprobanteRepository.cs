using ComprobantePago.Application.Commands.Comprobante;
using ComprobantePago.Application.Commands.Imputacion;
using ComprobantePago.Application.DTOs.Comprobante.Response;
using ComprobantePago.Application.Interfaces.Repositories;
using ComprobantePago.Application.Interfaces.Services;
using ComprobantePago.Domain.Entities;
using ComprobantePago.Infrastructure.Persistence;
using ComprobantePago.Infrastructure.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.IO.Compression;

namespace ComprobantePago.Infrastructure.Repositories
{
    public class ComprobanteRepository(
        AppDbContext contexto,
        ISunatService sunatService,
        XmlComprobanteService xmlService,
        PdfComprobanteService pdfService)
        : RepositorioBase<Comprobante>(contexto), IComprobanteRepository
    {
        private readonly ISunatService _sunatService = sunatService;
        private readonly XmlComprobanteService _xmlService = xmlService;
        private readonly PdfComprobanteService _pdfService = pdfService;

        // ── Generar Folio ─────────────────────────
        public async Task<string> GenerarFolioAsync()
        {
            var anio = DateTime.Now.Year.ToString();
            var mes = DateTime.Now.Month.ToString("D2");
            var correlativo = await ObtenerCorrelativoAsync(anio, mes);
            return $"{anio}{mes}{correlativo}";
        }

        private async Task<string> ObtenerCorrelativoAsync(
            string anio, string mes)
        {
            var prefijo = $"{anio}{mes}";

            var ultimo = await _contexto.Comprobantes
                .Where(x => x.Folio.StartsWith(prefijo))
                .OrderByDescending(x => x.Folio)
                .FirstOrDefaultAsync();

            int correlativo = 1;
            if (ultimo != null)
            {
                var sufijo = ultimo.Folio.Substring(prefijo.Length);
                if (int.TryParse(sufijo, out int num))
                    correlativo = num + 1;
            }

            return correlativo.ToString("D4");
        }

        // ── Guardar Comprobante ───────────────────
        public async Task<string> GuardarAsync(
            RegistrarComprobanteCommand command)
        {
            var dto = command.Comprobante;
            var folio = dto.Folio;

            var existente = await _contexto.Comprobantes
                .FirstOrDefaultAsync(x => x.Folio == folio);

            if (existente != null)
            {
                // ── Actualizar ────────────────────
                existente.RucReceptor = dto.Ruc;
                existente.RazonSocialReceptor = dto.RazonSocial;
                existente.TipoDocumento = dto.TipoDocumento;
                existente.TipoSunat = dto.TipoSunat;
                existente.Serie = dto.Serie;
                existente.Numero = dto.Numero;
                existente.FechaEmision = ParseFecha(dto.FechaEmision);
                existente.FechaRecepcion = ParseFechaNullable(dto.FechaRecepcion);
                existente.FechaVencimiento = ParseFechaNullable(dto.FechaVencimiento);
                existente.Moneda = dto.Moneda;
                existente.TasaCambio = dto.TasaCambio;
                existente.LugarPago = dto.LugarPago;
                existente.PlazoPago = dto.PlazoPago;
                existente.RucBeneficiario = dto.RucBenef;
                existente.RazonSocialBenef = dto.RazonSocial;
                existente.Observacion = dto.Observacion;
                existente.OrdenCompra = dto.OrdenCompra;
                existente.FactMultiple = dto.FactMultiple;
                existente.TieneDetraccion = dto.TieneDetraccion;
                existente.TipoDetraccion = dto.TipoDetraccion;
                existente.PorcentajeDetraccion = dto.PorcentajeDetraccion;
                existente.MontoDetraccion = dto.MontoDetraccion;
                existente.ConstanciaDeposito = dto.ConstanciaDeposito;
                existente.FechaDeposito = ParseFechaNullable(dto.FechaDeposito);
                existente.EsDocumentoElectronico =
                    dto.EsDocumentoElectronico == "S";
                existente.MontoNeto = dto.MontoNeto;
                existente.MontoExento = dto.MontoExento;
                existente.PorcentajeIGV = dto.PorcentajeIGV;
                existente.MontoIGVCredito = dto.MontoIGVCredito;
                existente.MontoTotal = dto.MontoTotal;
                existente.MontoBruto = dto.MontoBruto;
                existente.MontoRetencion = dto.MontoRetencion;
                existente.CodigoEstado = "REGISTRADO";
                existente.UsuarioAct = "SYSTEM";
                existente.FechaAct = DateTime.Now;

                await _contexto.SaveChangesAsync();
            }
            else
            {
                // ── Crear nuevo ───────────────────
                var comprobante = new Comprobante
                {
                    Folio = folio,
                    RucReceptor = dto.Ruc,
                    RazonSocialReceptor = dto.RazonSocial,
                    TipoDocumento = dto.TipoDocumento,
                    TipoSunat = dto.TipoSunat,
                    Serie = dto.Serie,
                    Numero = dto.Numero,
                    FechaEmision = ParseFecha(dto.FechaEmision),
                    FechaRecepcion = ParseFechaNullable(dto.FechaRecepcion),
                    FechaVencimiento = ParseFechaNullable(dto.FechaVencimiento),
                    Moneda = dto.Moneda,
                    TasaCambio = dto.TasaCambio,
                    LugarPago = dto.LugarPago,
                    PlazoPago = dto.PlazoPago,
                    RucBeneficiario = dto.RucBenef,
                    RazonSocialBenef = dto.RazonSocial,
                    Observacion = dto.Observacion,
                    OrdenCompra = dto.OrdenCompra,
                    FactMultiple = dto.FactMultiple,
                    TieneDetraccion = dto.TieneDetraccion,
                    TipoDetraccion = dto.TipoDetraccion,
                    PorcentajeDetraccion = dto.PorcentajeDetraccion,
                    MontoDetraccion = dto.MontoDetraccion,
                    ConstanciaDeposito = dto.ConstanciaDeposito,
                    FechaDeposito = ParseFechaNullable(dto.FechaDeposito),
                    EsDocumentoElectronico =
                        dto.EsDocumentoElectronico == "S",
                    MontoNeto = dto.MontoNeto,
                    MontoExento = dto.MontoExento,
                    PorcentajeIGV = dto.PorcentajeIGV,
                    MontoIGVCredito = dto.MontoIGVCredito,
                    MontoTotal = dto.MontoTotal,
                    MontoBruto = dto.MontoBruto,
                    MontoRetencion = dto.MontoRetencion,
                    CodigoEstado = "REGISTRADO",
                    UsuarioReg = "SYSTEM",
                    FechaReg = DateTime.Now
                };

                await _entidades.AddAsync(comprobante);
                await _contexto.SaveChangesAsync();
            }

            return folio;
        }

        // ── Enviar ────────────────────────────────
        public async Task EnviarAsync(
            EnviarComprobanteCommand command)
        {
            var c = await ObtenerPorFolioAsync(
                command.Comprobante.Folio);
            c.CodigoEstado = "ENVIADO";
            c.UsuarioAct = "SYSTEM";
            c.FechaAct = DateTime.Now;
            await _contexto.SaveChangesAsync();
        }

        // ── Firmar ────────────────────────────────
        public async Task FirmarAsync(
            FirmarComprobanteCommand command)
        {
            var c = await ObtenerPorFolioAsync(
                command.Comprobante.Folio);
            c.CodigoEstado = "AUTORIZADO";
            c.RolAutorizacion = "SYSTEM";
            c.UsuarioAct = "SYSTEM";
            c.FechaAct = DateTime.Now;
            await _contexto.SaveChangesAsync();
        }

        // ── Aprobar ───────────────────────────────
        public async Task AprobarAsync(
            AprobarComprobanteCommand command)
        {
            var c = await ObtenerPorFolioAsync(
                command.Comprobante.Folio);
            c.CodigoEstado = "APROBADO";
            c.RolAprobacion = "SYSTEM";
            c.FechaAprobacion = DateTime.Now;
            c.UsuarioAct = "SYSTEM";
            c.FechaAct = DateTime.Now;
            await _contexto.SaveChangesAsync();
        }

        // ── Anular ────────────────────────────────
        public async Task AnularAsync(
            AnularComprobanteCommand command)
        {
            var c = await ObtenerPorFolioAsync(
                command.Comprobante.Folio);
            c.CodigoEstado = "ANULADO";
            c.RolAnulacion = "SYSTEM";
            c.FechaAnulacion = DateTime.Now;
            c.UsuarioAct = "SYSTEM";
            c.FechaAct = DateTime.Now;
            await _contexto.SaveChangesAsync();
        }

        // ── Derivar ───────────────────────────────
        public async Task DerivarAsync(
            DerivarComprobanteCommand command)
        {
            var c = await ObtenerPorFolioAsync(
                command.Comprobante.Folio);
            c.EstaDerivado = true;
            c.UsuarioAct = "SYSTEM";
            c.FechaAct = DateTime.Now;
            await _contexto.SaveChangesAsync();
        }

        // ── Validar XML SUNAT ─────────────────────
        public async Task<ValidacionSunatDto> ValidarXmlSunatAsync(
            IFormFile archivo)
        {
            // Leer contenido para poder guardarlo después
            using var ms = new MemoryStream();
            await archivo.CopyToAsync(ms);
            var contenidoXml = ms.ToArray();

            using var stream = new MemoryStream(contenidoXml);
            var datos = _xmlService.LeerDatosXml(stream);

            var resultado = await _sunatService.ValidarComprobanteAsync(
                numRuc: datos.RucProveedor,
                codComp: datos.TipoSunat,
                numeroSerie: datos.Serie,
                numero: datos.Numero,
                fechaEmision: datos.FechaEmision,
                monto: datos.MontoTotal
            );

            if (resultado.CodigoEstado == "1" ||
                resultado.CodigoEstado == "3")
            {
                resultado.Datos = datos;
                resultado.Folio = await GenerarFolioAsync();

                // Guardar XML automáticamente al validar
                await GuardarDocumentosAsync(resultado.Folio,
                    new List<(byte[], string, string, string)>
                    {
                        (contenidoXml, archivo.FileName, "XML", "XML_SUNAT")
                    });
            }

            return resultado;
        }

        // ── Validar PDF SUNAT ─────────────────────
        public async Task<ValidacionSunatDto> ValidarPdfSunatAsync(
            IFormFile archivo)
        {
            var texto = await _pdfService.ExtraerTextoPdfAsync(archivo);

            if (string.IsNullOrWhiteSpace(texto))
                return new ValidacionSunatDto
                {
                    Exito = false,
                    EstadoSunat = "ERROR",
                    Motivo = "No se pudo extraer texto del PDF."
                };

            var datos = _pdfService.ExtraerDatosPdf(texto);

            if (datos == null)
                return new ValidacionSunatDto
                {
                    Exito = false,
                    EstadoSunat = "ERROR",
                    Motivo = "No se pudieron identificar los datos."
                };

            var resultado = await _sunatService.ValidarComprobanteAsync(
                numRuc: datos.RucProveedor,
                codComp: datos.TipoSunat,
                numeroSerie: datos.Serie,
                numero: datos.Numero,
                fechaEmision: datos.FechaEmision,
                monto: datos.MontoTotal
            );

            if (resultado.CodigoEstado == "1" ||
                resultado.CodigoEstado == "3")
            {
                resultado.Datos = datos;
                resultado.Folio = await GenerarFolioAsync();
            }

            return resultado;
        }

        // ── Imputaciones ──────────────────────────
        public async Task<ImputacionDetalleDto> AgregarImputacionAsync(
            AgregarImputacionCommand command)
        {
            var dto = command.Imputacion;
            var maxSec = await _contexto.ImputacionesContables
                .Where(x => x.Folio == dto.Folio)
                .MaxAsync(x => (int?)x.Secuencia) ?? 0;

            var entidad = MapearDtoAEntidad(dto, maxSec + 1);
            await _contexto.ImputacionesContables.AddAsync(entidad);
            await _contexto.SaveChangesAsync();
            return MapearEntidadADto(entidad);
        }

        public async Task<ImputacionDetalleDto> EditarImputacionAsync(
            EditarImputacionCommand command)
        {
            var dto = command.Imputacion;
            var secuencia = int.TryParse(dto.Secuencia, out var s) ? s : 0;

            var entidad = await _contexto.ImputacionesContables
                .FirstOrDefaultAsync(x => x.Folio == dto.Folio
                                       && x.Secuencia == secuencia)
                ?? throw new Exception(
                    $"Imputación {dto.Folio}/{secuencia} no encontrada.");

            entidad.AliasCuenta = dto.AliasCuenta;
            entidad.CuentaContable = dto.CuentaContable;
            entidad.DescripcionCuenta = dto.DescripcionCuenta;
            entidad.Monto = dto.Monto;
            entidad.Descripcion = dto.Descripcion;
            entidad.Proyecto = dto.Proyecto;
            entidad.CodUnidad1Cuenta = dto.CodUnidad1Cuenta;
            entidad.CodUnidad3Cuenta = dto.CodUnidad3Cuenta;
            entidad.CodUnidad4Cuenta = dto.CodUnidad4Cuenta;
            entidad.UsuarioAct = "SYSTEM";
            entidad.FechaAct = DateTime.Now;

            await _contexto.SaveChangesAsync();
            return MapearEntidadADto(entidad);
        }

        public async Task EliminarImputacionAsync(
            EliminarImputacionCommand command)
        {
            var entidad = await _contexto.ImputacionesContables
                .FirstOrDefaultAsync(x => x.Folio == command.Folio
                                       && x.Secuencia == command.Secuencia);
            if (entidad != null)
            {
                _contexto.ImputacionesContables.Remove(entidad);
                await _contexto.SaveChangesAsync();
            }
        }

        public Task<IEnumerable<ImputacionDetalleDto>>
            CargarImputacionMasivaAsync(IFormFile file)
            => Task.FromResult<IEnumerable<ImputacionDetalleDto>>(
                new List<ImputacionDetalleDto>());

        // ── Helpers imputación ────────────────────
        private static ImputacionContable MapearDtoAEntidad(
            Application.DTOs.Comprobante.Requests.ImputacionDto dto,
            int secuencia) => new()
        {
            Folio = dto.Folio,
            Secuencia = secuencia,
            AliasCuenta = dto.AliasCuenta,
            CuentaContable = dto.CuentaContable,
            DescripcionCuenta = dto.DescripcionCuenta,
            Monto = dto.Monto,
            Descripcion = dto.Descripcion,
            Proyecto = dto.Proyecto,
            CodUnidad1Cuenta = dto.CodUnidad1Cuenta,
            CodUnidad3Cuenta = dto.CodUnidad3Cuenta,
            CodUnidad4Cuenta = dto.CodUnidad4Cuenta,
            UsuarioReg = "SYSTEM",
            FechaReg = DateTime.Now
        };

        private static ImputacionDetalleDto MapearEntidadADto(
            ImputacionContable e) => new()
        {
            Secuencia = e.Secuencia,
            Folio = e.Folio,
            AliasCuenta = e.AliasCuenta ?? string.Empty,
            CuentaContable = e.CuentaContable ?? string.Empty,
            DescripcionCuenta = e.DescripcionCuenta ?? string.Empty,
            Monto = e.Monto,
            Descripcion = e.Descripcion ?? string.Empty,
            Proyecto = e.Proyecto ?? string.Empty,
            CodUnidad1Cuenta = e.CodUnidad1Cuenta ?? string.Empty,
            CodUnidad3Cuenta = e.CodUnidad3Cuenta ?? string.Empty,
            CodUnidad4Cuenta = e.CodUnidad4Cuenta ?? string.Empty
        };

        // ── Validar ZIP SUNAT ─────────────────────
        public async Task<ValidacionSunatDto> ValidarZipSunatAsync(
            IFormFile archivo)
        {
            using var zipStream = archivo.OpenReadStream();
            using var zip = new ZipArchive(zipStream, ZipArchiveMode.Read);

            // Buscar XML que no sea CDR (nombre no empieza con R-)
            var entradaXml = zip.Entries.FirstOrDefault(e =>
                e.Name.EndsWith(".xml", StringComparison.OrdinalIgnoreCase) &&
                !e.Name.StartsWith("R-", StringComparison.OrdinalIgnoreCase));

            if (entradaXml == null)
                return new ValidacionSunatDto
                {
                    Exito = false,
                    EstadoSunat = "ERROR",
                    Motivo = "El ZIP no contiene un archivo XML de comprobante."
                };

            using var xmlStream = entradaXml.Open();
            var datos = _xmlService.LeerDatosXml(xmlStream);

            var resultado = await _sunatService.ValidarComprobanteAsync(
                numRuc: datos.RucProveedor,
                codComp: datos.TipoSunat,
                numeroSerie: datos.Serie,
                numero: datos.Numero,
                fechaEmision: datos.FechaEmision,
                monto: datos.MontoTotal
            );

            if (resultado.CodigoEstado == "1" ||
                resultado.CodigoEstado == "3")
            {
                resultado.Datos = datos;
                resultado.Folio = await GenerarFolioAsync();

                // Guardar todos los archivos del ZIP automáticamente
                var archivos = new List<(byte[] contenido, string nombre, string tipo, string subTipo)>();
                foreach (var entrada in zip.Entries)
                {
                    var ext = Path.GetExtension(entrada.Name)
                        .TrimStart('.').ToLowerInvariant();
                    if (ext != "xml" && ext != "pdf") continue;

                    using var ms = new MemoryStream();
                    using var entStream = entrada.Open();
                    await entStream.CopyToAsync(ms);

                    var esCdr = entrada.Name.StartsWith("R-",
                        StringComparison.OrdinalIgnoreCase);
                    string tipo, subTipo;
                    if (ext == "pdf")        { tipo = "PDF"; subTipo = "REPRESENTACION_IMPRESA"; }
                    else if (esCdr)          { tipo = "XML"; subTipo = "XML_CDR"; }
                    else                     { tipo = "XML"; subTipo = "XML_SUNAT"; }
                    archivos.Add((ms.ToArray(), entrada.Name, tipo, subTipo));
                }
                await GuardarDocumentosAsync(resultado.Folio, archivos);
            }

            return resultado;
        }

        // ── Guardar Documentos Electrónicos ──────
        public async Task GuardarDocumentosAsync(
            string folio,
            List<(byte[] contenido, string nombre, string tipo, string subTipo)> archivos)
        {
            foreach (var (contenido, nombre, tipo, subTipo) in archivos)
            {
                var doc = new DocumentoElectronico
                {
                    Folio = folio,
                    TipoArchivo = tipo,
                    SubTipo = subTipo,
                    NombreArchivo = nombre,
                    Contenido = contenido,
                    FechaReg = DateTime.Now,
                    UsuarioReg = "SYSTEM"
                };
                await _contexto.DocumentosElectronicos.AddAsync(doc);
            }
            await _contexto.SaveChangesAsync();
        }

        // ── Descargar Documento Electrónico ───────
        public async Task<DocumentoElectronico?> DescargarDocumentoAsync(
            int idDocumento)
        {
            return await _contexto.DocumentosElectronicos
                .FirstOrDefaultAsync(x => x.IdDocumento == idDocumento);
        }

        // ── Eliminar Documento Electrónico ────────
        public async Task EliminarDocumentoAsync(int idDocumento)
        {
            var doc = await _contexto.DocumentosElectronicos
                .FirstOrDefaultAsync(x => x.IdDocumento == idDocumento);
            if (doc != null)
            {
                _contexto.DocumentosElectronicos.Remove(doc);
                await _contexto.SaveChangesAsync();
            }
        }

        // ── Helpers ───────────────────────────────
        private async Task<Comprobante> ObtenerPorFolioAsync(
            string folio)
        {
            return await _contexto.Comprobantes
                .FirstOrDefaultAsync(x => x.Folio == folio)
                ?? throw new Exception(
                    $"Comprobante {folio} no encontrado.");
        }

        private static DateTime ParseFecha(string? fecha)
        {
            return DateTime.TryParse(fecha, out var result)
                ? result : DateTime.Now;
        }

        private static DateTime? ParseFechaNullable(string? fecha)
        {
            if (string.IsNullOrEmpty(fecha)) return null;
            return DateTime.TryParse(fecha, out var result)
                ? result : null;
        }
    }
}