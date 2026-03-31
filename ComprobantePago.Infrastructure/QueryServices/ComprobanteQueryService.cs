using AutoMapper;
using ComprobantePago.Application.DTOs.Comprobante.Common;
using ComprobantePago.Application.DTOs.Comprobante.Requests;
using ComprobantePago.Application.DTOs.Comprobante.Response;
using ComprobantePago.Application.DTOs.Responses;
using ComprobantePago.Application.Interfaces.QueryServices;
using ComprobantePago.Application.Interfaces.Services.Maestros;
using ComprobantePago.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ComprobantePago.Infrastructure.QueryServices
{
    public class ComprobanteQueryService(
        AppDbContext contexto,
        IMapper mapper,
        ILogger<ComprobanteQueryService> logger,
        IEmpleadoService empleadoService,
        ICatalogoUnidadService cataloService,
        ICuentaContableService cuentaService)
        : IComprobanteQueryService
    {
        private readonly AppDbContext _contexto = contexto;
        private readonly IMapper _mapper = mapper;
        private readonly ILogger<ComprobanteQueryService> _logger = logger;
        private readonly IEmpleadoService _empleadoService = empleadoService;
        private readonly ICatalogoUnidadService _cataloService = cataloService;
        private readonly ICuentaContableService _cuentaService = cuentaService;

        #region Combos
        public async Task<IEnumerable<ComboDto>> ObtenerTiposDocumentoAsync()
            => await _contexto.TiposDocumento
                .Where(x => x.Activo)
                .OrderBy(x => x.Codigo)
                .Select(x => new ComboDto
                {
                    Codigo = x.Codigo,
                    Descripcion = x.Descripcion
                })
                .ToListAsync();

        public async Task<IEnumerable<ComboDto>> ObtenerTiposSunatAsync()
            => await _contexto.TiposSunat
                .Where(x => x.Activo)
                .OrderBy(x => x.Codigo)
                .Select(x => new ComboDto
                {
                    Codigo = x.Codigo,
                    Descripcion = x.Descripcion
                })
                .ToListAsync();

        public async Task<IEnumerable<ComboDto>> ObtenerMonedasAsync()
            => await _contexto.Monedas
                .Where(x => x.Activo)
                .OrderBy(x => x.Codigo)
                .Select(x => new ComboDto
                {
                    Codigo = x.Codigo,
                    Descripcion = x.Descripcion
                })
                .ToListAsync();

        public async Task<IEnumerable<ComboDto>> ObtenerLugaresPagoAsync()
            => await _contexto.LugaresPago
                .Where(x => x.Activo)
                .OrderBy(x => x.Codigo)
                .Select(x => new ComboDto
                {
                    Codigo = x.Codigo,
                    Descripcion = x.Descripcion
                })
                .ToListAsync();

        public async Task<IEnumerable<ComboDto>> ObtenerTiposDetraccionAsync()
            => await _contexto.TiposDetraccion
                .Where(x => x.Activo)
                .OrderBy(x => x.Codigo)
                .Select(x => new ComboDto
                {
                    Codigo = x.Codigo,
                    Descripcion = $"{x.Codigo} - {x.Descripcion}",
                    Porcentaje = x.Porcentaje
                })
                .ToListAsync();

        public async Task<IEnumerable<ComboDto>> ObtenerEstadosAsync()
            => await _contexto.EstadosComprobante
                .Where(x => x.Activo)
                .OrderBy(x => x.Codigo)
                .Select(x => new ComboDto
                {
                    Codigo = x.Codigo,
                    Descripcion = x.Descripcion
                })
                .ToListAsync();

        public Task<IEnumerable<ComboDto>> ObtenerEmpleadosAsync(string filtro = "")
            => _empleadoService.ObtenerEmpleadosAsync(filtro);

        public Task<IEnumerable<ComboDto>> ObtenerCuentasContablesAsync(string filtro = "")
            => _cuentaService.ObtenerCuentasContablesAsync(filtro);

        public Task<IEnumerable<ComboDto>> ObtenerCodigosUnidadAsync(
            string campo, int unidad, string codigo, string filtro = "")
            => _cataloService.ObtenerCodigosUnidadAsync(unidad, filtro);

        #endregion

        #region Buscar y Detalle

        // ── Buscar comprobantes ───────────────────
        public async Task<IEnumerable<ComprobanteDto>> BuscarAsync(
            BuscarComprobanteDto filtros)
        {
            _logger.LogInformation("Buscando comprobantes con filtros: {@Filtros}", filtros);
            var query = _contexto.Comprobantes.AsQueryable();

            if (!string.IsNullOrEmpty(filtros.Tipo))
                query = query.Where(x =>
                    x.TipoDocumento == filtros.Tipo);

            if (!string.IsNullOrEmpty(filtros.Estado))
                query = query.Where(x =>
                    x.CodigoEstado == filtros.Estado);

            if (!string.IsNullOrEmpty(filtros.Proveedor))
                query = query.Where(x =>
                    x.RucReceptor.Contains(filtros.Proveedor) ||
                    x.RazonSocialReceptor.Contains(filtros.Proveedor));

            if (!string.IsNullOrEmpty(filtros.Folio))
                query = query.Where(x =>
                    x.Folio.Contains(filtros.Folio));

            return await query
                .OrderByDescending(x => x.FechaReg)
                .Select(x => new ComprobanteDto
                {
                    Folio = x.Folio,
                    TipoComprobante = x.TipoDocumento,
                    Serie = x.Serie,
                    Numero = x.Numero,
                    Proveedor = x.RazonSocialReceptor,
                    Fecha = x.FechaEmision
                                       .ToString("dd/MM/yyyy"),
                    Moneda = x.Moneda,
                    MontoTotal = x.MontoTotal,
                    Estado = x.CodigoEstado
                })
                .ToListAsync();
        }

        // ── Obtener detalle ───────────────────────
        public async Task<ComprobanteDetalleDto> ObtenerDetalleAsync(
            string folio)
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
                FechaEmision = c.FechaEmision
                                        .ToString("yyyy-MM-dd"),
                FechaRecepcion = c.FechaRecepcion?
                                        .ToString("yyyy-MM-dd"),
                Moneda = c.Moneda,
                TasaCambio = c.TasaCambio,
                LugarPago = c.LugarPago,
                PlazoPago = c.PlazoPago,
                FechaVencimiento = c.FechaVencimiento?
                                        .ToString("yyyy-MM-dd"),
                RucBenef = c.RucBeneficiario,
                RazonSocialBenef = c.RazonSocialBenef,
                Observacion = c.Observacion,
                OrdenCompra = c.OrdenCompra,
                FactMultiple = c.FactMultiple,
                TieneDetraccion = c.TieneDetraccion,
                TipoDetraccion = c.TipoDetraccion,
                PorcentajeDetraccion = c.PorcentajeDetraccion ?? 0,
                ConstanciaDeposito = c.ConstanciaDeposito,
                FechaDeposito = c.FechaDeposito?
                                        .ToString("yyyy-MM-dd"),
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
                FechaAprobacion = c.FechaAprobacion?
                                        .ToString("yyyy-MM-dd"),
                RolAnulacion = c.RolAnulacion,
                FechaAnulacion = c.FechaAnulacion?
                                        .ToString("yyyy-MM-dd"),
                Mensaje = c.Mensaje,
                EsEmpleado     = c.EsEmpleado,
                EmpleadoCodigo = c.EmpleadoCodigo ?? string.Empty,
                EmpleadoNombre = c.EmpleadoNombre ?? string.Empty,
                RequiereAduana = c.RequiereAduana,
                RequiereDetraccion = c.RequiereDetraccion,
                EsDocumentoElectronico =
                    c.EsDocumentoElectronico ? "S" : "N"
            };
        }
        #endregion

        #region Pendientes
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
        #endregion

        #region Distribución Syteline
        public async Task<IEnumerable<SytelineDistribucionDto>>
            ObtenerDistribucionSytelineAsync(List<string>? folios = null)
        {
            var query = _contexto.Comprobantes
                .Where(x => x.CodigoEstado == "APROBADO");

            if (folios != null && folios.Any())
                query = query.Where(x => folios.Contains(x.Folio));

            var comprobantes = await query
                .OrderBy(x => x.FechaAprobacion)
                .ToListAsync();

            var resultado = new List<SytelineDistribucionDto>();

            foreach (var c in comprobantes)
            {
                var imputaciones = await _contexto.ImputacionesContables
                    .Where(x => x.Folio == c.Folio)
                    .OrderBy(x => x.Secuencia)
                    .ToListAsync();

                if (!imputaciones.Any()) continue;

                // Imputación 1 → cabecera; imputación 2 → base imponible;
                // imputación 3 → IGV; imputación 4 → exento (solo si aplica)
                var tieneExento = c.MontoExento > 0;
                var cantDist    = tieneExento ? 3 : 2;

                var imputacionesDistribucion = imputaciones.Skip(1).Take(cantDist).ToList();
                if (imputacionesDistribucion.Count < 2) continue; // mínimo base + IGV

                var fechaDist = c.FechaRecepcion.HasValue
                    ? c.FechaRecepcion.Value.ToString("d/MM/yyyy")
                    : c.FechaEmision.ToString("d/MM/yyyy");

                // Cada imputación genera exactamente 1 fila
                for (int idx = 0; idx < imputacionesDistribucion.Count; idx++)
                {
                    var imp = imputacionesDistribucion[idx];
                    int secDist = (idx + 1) * 5; // 5, 10, 15

                    string sistImpst, codImp, descCodImp;
                    decimal baseImp, importe;

                    switch (idx)
                    {
                        case 0: // Base imponible — cuenta de gasto/costo
                            sistImpst  = string.Empty;
                            codImp     = string.Empty;
                            descCodImp = string.Empty;
                            baseImp    = 0;
                            importe    = c.MontoNeto;
                            break;
                        case 1: // IGV
                            sistImpst  = "2";
                            codImp     = "IGV18";
                            descCodImp = "IGV 18%";
                            baseImp    = c.MontoNeto;
                            importe    = c.MontoIGVCredito;
                            break;
                        case 2: // Exento (solo cuando MontoExento > 0 y PorcentajeIGV > 0)
                            sistImpst  = string.Empty;
                            codImp     = "EXO";
                            descCodImp = "Exento";
                            baseImp    = c.MontoExento;
                            importe    = c.MontoExento;
                            break;
                        default:
                            sistImpst  = string.Empty;
                            codImp     = string.Empty;
                            descCodImp = string.Empty;
                            baseImp    = 0;
                            importe    = imp.Monto;
                            break;
                    }

                    // When employee is selected, use employee code/name as proveedor/nombre
                    // and XML provider data fills the "Is Third Party?" fields
                    var proveedorExport = c.EsEmpleado ? (c.EmpleadoCodigo ?? string.Empty) : c.RucReceptor;
                    var nombreExport    = c.EsEmpleado ? (c.EmpleadoNombre ?? string.Empty) : c.RazonSocialReceptor;

                    // Is Third Party / Nro proveedor: from XML provider when EsEmpleado, else from beneficiario
                    var nroProvDist   = c.EsEmpleado && idx == 0 ? c.RucReceptor        : (idx == 0 ? (c.RucBeneficiario ?? string.Empty) : string.Empty);
                    var nomProvDist   = c.EsEmpleado && idx == 0 ? c.RazonSocialReceptor : (idx == 0 ? (c.RazonSocialBenef ?? string.Empty) : string.Empty);
                    var numRegFiscDist = c.EsEmpleado && idx == 0 ? c.RucReceptor        : (idx == 0 ? (c.RucBeneficiario ?? string.Empty) : string.Empty);

                    resultado.Add(new SytelineDistribucionDto
                    {
                        Proveedor         = proveedorExport,
                        Comprobante       = c.IdComprobante,
                        Nombre            = nombreExport,
                        FechaDistribucion = fechaDist,
                        Factura           = $"{c.Serie}-{c.Numero}",
                        FechaFactura      = c.FechaEmision.ToString("d/MM/yyyy"),
                        TasaCambio        = c.TasaCambio,
                        Moneda            = c.Moneda,
                        ImpoCompra        = c.MontoNeto,
                        IGV               = c.MontoIGVCredito,
                        MntoFactura       = c.MontoTotal,
                        TotalDistribucion = c.MontoTotal,
                        NroProveedor      = nroProvDist,
                        NombreProv        = nomProvDist,
                        NumRegFiscal      = numRegFiscDist,
                        SecDist           = secDist,
                        Proyecto          = imp.Proyecto ?? string.Empty,
                        SistImpst         = sistImpst,
                        CodImp            = codImp,
                        DescCodImp        = descCodImp,
                        BaseImp           = baseImp,
                        Importe           = importe,
                        CuentaContable    = imp.CuentaContable ?? string.Empty,
                        DescripcionCuenta = imp.DescripcionCuenta ?? string.Empty,
                        CodUnidad1        = imp.CodUnidad1Cuenta ?? string.Empty,
                        CodUnidad3        = imp.CodUnidad3Cuenta ?? string.Empty,
                        CodUnidad4        = imp.CodUnidad4Cuenta ?? string.Empty,
                        EsLineaPrincipal  = idx == 0,
                        TipoDoc           = c.TipoSunat
                    });
                }
            }

            return resultado;
        }
        #endregion

        #region Syteline
        public async Task<IEnumerable<SytelineCabeceraDto>>
            ObtenerCabecerasSytelineAsync(List<string>? folios = null)
        {
            var query = _contexto.Comprobantes
                .Where(x => x.CodigoEstado == "APROBADO");

            // ← filtrar por folios seleccionados
            if (folios != null && folios.Any())
                query = query.Where(x => folios.Contains(x.Folio));

            var comprobantes = await query
                .OrderBy(x => x.FechaAprobacion)
                .ToListAsync();

            var result = new List<SytelineCabeceraDto>();

            foreach (var c in comprobantes)
            {
                var primeraImp = await _contexto.ImputacionesContables
                    .Where(i => i.Folio == c.Folio)
                    .OrderBy(i => i.Secuencia)
                    .FirstOrDefaultAsync();

                var finMes = new DateTime(
                    c.FechaEmision.Year,
                    c.FechaEmision.Month,
                    DateTime.DaysInMonth(
                        c.FechaEmision.Year,
                        c.FechaEmision.Month));

                result.Add(new SytelineCabeceraDto
                {
                    Proveedor = c.EsEmpleado ? (c.EmpleadoCodigo ?? string.Empty) : c.RucReceptor,
                    Nombre    = c.EsEmpleado ? (c.EmpleadoNombre ?? string.Empty) : c.RazonSocialReceptor,
                    Comprobante = c.IdComprobante,
                    Factura = $"{c.Serie}-{c.Numero}",
                    FechaFactura = c.FechaEmision
                                        .ToString("dd/MM/yyyy"),
                    FechaDistribucion = c.FechaRecepcion.HasValue
                                        ? c.FechaRecepcion.Value
                                          .ToString("dd/MM/yyyy")
                                        : c.FechaEmision
                                          .ToString("dd/MM/yyyy"),
                    ImpoCompra = c.MontoNeto,
                    CargosVarios = c.MontoExento,
                    ImpVentas2 = c.MontoIGVCredito,
                    MntoFactura = c.MontoBruto,
                    ImpSinDesc = c.MontoIGVCredito,
                    FechaDcto = finMes.ToString("dd/MM/yyyy"),
                    DiasVto = int.TryParse(
                                        c.PlazoPago, out var dias)
                                        ? dias : 0,
                    FechaVen = c.FechaVencimiento.HasValue
                                        ? c.FechaVencimiento.Value
                                          .ToString("dd/MM/yyyy")
                                        : string.Empty,
                    Moneda = c.Moneda,
                    TipoCambio = c.TasaCambio,
                    CtaCP = primeraImp?.CuentaContable
                                        ?? string.Empty,
                    CtaCPUnid1 = primeraImp?.CodUnidad1Cuenta
                                        ?? string.Empty,
                    CtaCPUnid2 = string.Empty,
                    CtaCPUnid3 = primeraImp?.CodUnidad3Cuenta
                                        ?? string.Empty,
                    CtaCPUnid4 = primeraImp?.CodUnidad4Cuenta
                                        ?? string.Empty,
                    DescripcionCuenta = primeraImp?.DescripcionCuenta
                                        ?? string.Empty,
                    Ref = c.Observacion ?? string.Empty,
                    EstadoAut = "Autorizado",
                    Autorizo = c.RolAutorizacion ?? string.Empty,
                    Notas = c.Mensaje ?? string.Empty,
                    UsaDetraccion = c.TieneDetraccion ? "1" : "",
                    Detraccion = c.TipoDetraccion ?? string.Empty,
                    Tasa = c.PorcentajeDetraccion ?? 0,
                    TotalDetraccion = c.MontoDetraccion,
                    TotalDetLocal = c.MontoDetraccion,
                    MontoExento    = c.MontoExento,
                    MontoRetencion = c.MontoRetencion,
                    PorcentajeIGV  = c.PorcentajeIGV
                });
            }

            return result;
        }
        #endregion
    }
}