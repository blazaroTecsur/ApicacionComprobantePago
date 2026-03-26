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
            using var stream = archivo.OpenReadStream();
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

        // ── Imputaciones (pendiente) ──────────────
        public Task<ImputacionDetalleDto> AgregarImputacionAsync(
            AgregarImputacionCommand command)
            => Task.FromResult(new ImputacionDetalleDto());

        public Task<ImputacionDetalleDto> EditarImputacionAsync(
            EditarImputacionCommand command)
            => Task.FromResult(new ImputacionDetalleDto());

        public Task EliminarImputacionAsync(
            EliminarImputacionCommand command)
            => Task.CompletedTask;

        public Task<IEnumerable<ImputacionDetalleDto>>
            CargarImputacionMasivaAsync(IFormFile file)
            => Task.FromResult<IEnumerable<ImputacionDetalleDto>>(
                new List<ImputacionDetalleDto>());

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