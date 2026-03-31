using AutoMapper;
using ComprobantePago.Application.DTOs.Comprobante.Requests;
using ComprobantePago.Application.DTOs.Comprobante.Response;
using ComprobantePago.Application.DTOs.Responses;
using ComprobantePago.Application.Interfaces.QueryServices;
using ComprobantePago.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ComprobantePago.Infrastructure.QueryServices
{
    public class ComprobanteQueryService(
        AppDbContext contexto,
        IMapper mapper,
        ILogger<ComprobanteQueryService> logger)
        : IComprobanteQueryService
    {
        private readonly AppDbContext _contexto = contexto;
        private readonly IMapper _mapper = mapper;
        private readonly ILogger<ComprobanteQueryService> _logger = logger;

        // ── Buscar comprobantes ───────────────────
        public async Task<IEnumerable<ComprobanteDto>> BuscarAsync(
            BuscarComprobanteDto filtros)
        {
            _logger.LogInformation("Buscando comprobantes con filtros: {@Filtros}", filtros);
            var query = _contexto.Comprobantes.AsQueryable();

            if (!string.IsNullOrEmpty(filtros.Tipo))
                query = query.Where(x => x.TipoDocumento == filtros.Tipo);

            if (!string.IsNullOrEmpty(filtros.Estado))
                query = query.Where(x => x.CodigoEstado == filtros.Estado);

            if (!string.IsNullOrEmpty(filtros.Proveedor))
                query = query.Where(x =>
                    x.RucReceptor.Contains(filtros.Proveedor) ||
                    x.RazonSocialReceptor.Contains(filtros.Proveedor));

            if (!string.IsNullOrEmpty(filtros.Folio))
                query = query.Where(x => x.Folio.Contains(filtros.Folio));

            return await query
                .OrderByDescending(x => x.FechaReg)
                .Select(x => new ComprobanteDto
                {
                    Folio = x.Folio,
                    TipoComprobante = x.TipoDocumento,
                    Serie = x.Serie,
                    Numero = x.Numero,
                    Proveedor = x.RazonSocialReceptor,
                    Fecha = x.FechaEmision.ToString("dd/MM/yyyy"),
                    Moneda = x.Moneda,
                    MontoTotal = x.MontoTotal,
                    Estado = x.CodigoEstado
                })
                .ToListAsync();
        }

        // ── Obtener detalle ───────────────────────
        public async Task<ComprobanteDetalleDto> ObtenerDetalleAsync(string folio)
        {
            var c = await _contexto.Comprobantes
                .FirstOrDefaultAsync(x => x.Folio == folio);

            if (c == null) return null!;

            return new ComprobanteDetalleDto
            {
                Folio = c.Folio,
                Ruc = c.RucReceptor,
                RazonSocial = c.RazonSocialReceptor,
                TipoDocumento = c.TipoDocumento,
                TipoSunat = c.TipoSunat,
                Serie = c.Serie,
                Numero = c.Numero,
                FechaEmision = c.FechaEmision.ToString("yyyy-MM-dd"),
                FechaRecepcion = c.FechaRecepcion?.ToString("yyyy-MM-dd"),
                Moneda = c.Moneda,
                TasaCambio = c.TasaCambio,
                LugarPago = c.LugarPago,
                PlazoPago = c.PlazoPago,
                FechaVencimiento = c.FechaVencimiento?.ToString("yyyy-MM-dd"),
                RucBenef = c.RucBeneficiario,
                RazonSocialBenef = c.RazonSocialBenef,
                Observacion = c.Observacion,
                OrdenCompra = c.OrdenCompra,
                FactMultiple = c.FactMultiple,
                TieneDetraccion = c.TieneDetraccion,
                TipoDetraccion = c.TipoDetraccion,
                PorcentajeDetraccion = c.PorcentajeDetraccion ?? 0,
                ConstanciaDeposito = c.ConstanciaDeposito,
                FechaDeposito = c.FechaDeposito?.ToString("yyyy-MM-dd"),
                MontoNeto = c.MontoNeto,
                MontoExento = c.MontoExento,
                MontoIGVCosto = c.MontoIGVCosto,
                PorcentajeIGV = c.PorcentajeIGV,
                MontoIGVCredito = c.MontoIGVCredito,
                MontoTotal = c.MontoTotal,
                MontoBruto = c.MontoBruto,
                MontoRetencion = c.MontoRetencion,
                MontoDetraccion = c.MontoDetraccion,
                CodigoEstado = c.CodigoEstado,
                Estado = c.CodigoEstado,
                RolDigitacion = c.RolDigitacion,
                RolAutorizacion = c.RolAutorizacion,
                RolAprobacion = c.RolAprobacion,
                FechaAprobacion = c.FechaAprobacion?.ToString("yyyy-MM-dd"),
                RolAnulacion = c.RolAnulacion,
                FechaAnulacion = c.FechaAnulacion?.ToString("yyyy-MM-dd"),
                Mensaje = c.Mensaje,
                EsEmpleado     = c.EsEmpleado,
                EmpleadoCodigo = c.EmpleadoCodigo ?? string.Empty,
                EmpleadoNombre = c.EmpleadoNombre ?? string.Empty,
                RequiereAduana = c.RequiereAduana,
                RequiereDetraccion = c.RequiereDetraccion,
                EsDocumentoElectronico = c.EsDocumentoElectronico ? "S" : "N"
            };
        }

        public Task<byte[]> ObtenerPdfAsync(string folio)
            => Task.FromResult<byte[]>(null!);

        public async Task<IEnumerable<DocumentoElectronicoDto>>
            ObtenerDocumentosElectronicosAsync(string folio)
        {
            return await _contexto.DocumentosElectronicos
                .Where(x => x.Folio == folio)
                .OrderBy(x => x.FechaReg)
                .Select(x => new DocumentoElectronicoDto
                {
                    IdDocumento = x.IdDocumento,
                    TipoArchivo = x.TipoArchivo,
                    SubTipo = x.SubTipo,
                    NombreArchivo = x.NombreArchivo,
                    FechaReg = x.FechaReg.ToString("dd/MM/yyyy HH:mm"),
                    TamanioBytes = x.Contenido.Length
                })
                .ToListAsync();
        }

        public async Task<IEnumerable<ImputacionDetalleDto>>
            ObtenerImputacionesAsync(string folio)
        {
            _logger.LogInformation("Obteniendo imputaciones para folio {Folio}", folio);

            var lista = await _contexto.ImputacionesContables
                .Where(x => x.Folio == folio)
                .OrderBy(x => x.Secuencia)
                .ToListAsync();

            return _mapper.Map<IEnumerable<ImputacionDetalleDto>>(lista);
        }

        public Task<byte[]> ObtenerPlantillaImputacionAsync()
            => Task.FromResult<byte[]>(null!);
    }
}
