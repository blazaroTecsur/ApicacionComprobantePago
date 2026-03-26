using ComprobantePago.Application.DTOs.Comprobante.Common;
using ComprobantePago.Application.DTOs.Comprobante.Requests;
using ComprobantePago.Application.DTOs.Comprobante.Response;
using ComprobantePago.Application.DTOs.Responses;
using ComprobantePago.Application.Interfaces.QueryServices;
using ComprobantePago.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace ComprobantePago.Infrastructure.QueryServices
{
    public class ComprobanteQueryService(AppDbContext contexto)
        : IComprobanteQueryService
    {
        private readonly AppDbContext _contexto = contexto;

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

        public async Task<IEnumerable<ComboDto>> ObtenerCodigosUnidadAsync(
            string campo, int unidad, string codigo)
        {
            return unidad switch
            {
                1 => await _contexto.CodigosUnidad1
                    .Where(x => x.Activo)
                    .OrderBy(x => x.Codigo)
                    .Select(x => new ComboDto
                    {
                        Codigo = x.Codigo,
                        Descripcion = x.Descripcion
                    })
                    .ToListAsync(),

                3 => await _contexto.CodigosUnidad3
                    .Where(x => x.Activo)
                    .OrderBy(x => x.Codigo)
                    .Select(x => new ComboDto
                    {
                        Codigo = x.Codigo,
                        Descripcion = x.Descripcion
                    })
                    .ToListAsync(),

                4 => await _contexto.CodigosUnidad4
                    .Where(x => x.Activo)
                    .OrderBy(x => x.Codigo)
                    .Select(x => new ComboDto
                    {
                        Codigo = x.Codigo,
                        Descripcion = x.Descripcion
                    })
                    .ToListAsync(),

                _ => new List<ComboDto>()
            };
        }
        #endregion

        #region Buscar y Detalle

        // ── Buscar comprobantes ───────────────────
        public async Task<IEnumerable<ComprobanteDto>> BuscarAsync(
            BuscarComprobanteDto filtros)
        {
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
            var lista = await _contexto.ImputacionesContables
                .Where(x => x.Folio == folio)
                .OrderBy(x => x.Secuencia)
                .ToListAsync();

            return lista.Select(e => new ImputacionDetalleDto
            {
                Secuencia = e.Secuencia,
                Folio = e.Folio,
                AliasCuenta = e.AliasCuenta ?? string.Empty,
                CuentaContable = e.CuentaContable ?? string.Empty,
                DescripcionCuenta = e.DescripcionCuenta ?? string.Empty,
                Monto = e.Monto,
                Descripcion = e.Descripcion ?? string.Empty,
                Referencia = e.Referencia ?? string.Empty,
                Not = e.NOT_Campo ?? string.Empty,
                Fondo = e.Fondo ?? string.Empty,
                Dividendo = e.Dividendo ?? string.Empty,
                Varios = e.Varios ?? string.Empty,
                Actividad = e.Actividad ?? string.Empty,
                CentroResponsabilidad = e.CentroResponsabilidad ?? string.Empty,
                Proyecto = e.Proyecto ?? string.Empty,
                CalidadRed = e.CalidadRed ?? string.Empty,
                UbicacionGeografica = e.UbicacionGeografica ?? string.Empty,
                SubRecurso = e.SubRecurso ?? string.Empty,
                ActividadIngreso = e.ActividadIngreso ?? string.Empty,
                Cajero = e.Cajero ?? string.Empty,
                Proveedor = e.Proveedor ?? string.Empty,
                JerarquiaCargo = e.JerarquiaCargo ?? string.Empty,
                CodUnidad1Cuenta = e.CodUnidad1Cuenta ?? string.Empty,
                CodUnidad3Cuenta = e.CodUnidad3Cuenta ?? string.Empty,
                CodUnidad4Cuenta = e.CodUnidad4Cuenta ?? string.Empty,
                CodUnidad1Actividad = e.CodUnidad1Actividad ?? string.Empty,
                CodUnidad3Actividad = e.CodUnidad3Actividad ?? string.Empty,
                CodUnidad4Actividad = e.CodUnidad4Actividad ?? string.Empty,
                CodUnidad1SubRecurso = e.CodUnidad1SubRecurso ?? string.Empty,
                CodUnidad3SubRecurso = e.CodUnidad3SubRecurso ?? string.Empty,
                CodUnidad4SubRecurso = e.CodUnidad4SubRecurso ?? string.Empty
            });
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

                var totalDist = imputaciones.Sum(i => i.Monto);
                var fechaDist = c.FechaRecepcion.HasValue
                    ? c.FechaRecepcion.Value.ToString("d/MM/yyyy")
                    : c.FechaEmision.ToString("d/MM/yyyy");

                for (int idx = 0; idx < imputaciones.Count; idx++)
                {
                    var imp = imputaciones[idx];
                    int baseSeq = idx * 15;

                    // Subfila A — principal
                    resultado.Add(new SytelineDistribucionDto
                    {
                        Proveedor = c.RucReceptor,
                        Comprobante = c.IdComprobante,
                        Nombre = c.RazonSocialReceptor,
                        FechaDistribucion = fechaDist,
                        Factura = $"{c.Serie}-{c.Numero}",
                        FechaFactura = c.FechaEmision.ToString("d/MM/yyyy"),
                        TasaCambio = c.TasaCambio,
                        Moneda = c.Moneda,
                        ImpoCompra = c.MontoNeto,
                        IGV = c.MontoIGVCredito,
                        MntoFactura = c.MontoBruto,
                        TotalDistribucion = totalDist,
                        NroProveedor = c.RucBeneficiario ?? string.Empty,
                        NombreProv = c.RazonSocialBenef ?? string.Empty,
                        NumRegFiscal = c.RucBeneficiario ?? string.Empty,
                        SecDist = baseSeq + 5,
                        Proyecto = imp.Proyecto ?? string.Empty,
                        SistImpst = string.Empty,
                        CodImp = string.Empty,
                        DescCodImp = string.Empty,
                        BaseImp = 0,
                        Importe = imp.Monto,
                        CuentaContable = imp.CuentaContable ?? string.Empty,
                        DescripcionCuenta = imp.DescripcionCuenta ?? string.Empty,
                        CodUnidad1 = imp.CodUnidad1Cuenta ?? string.Empty,
                        CodUnidad3 = imp.CodUnidad3Cuenta ?? string.Empty,
                        CodUnidad4 = imp.CodUnidad4Cuenta ?? string.Empty,
                        EsLineaPrincipal = true
                    });

                    // Subfila B — NR
                    resultado.Add(new SytelineDistribucionDto
                    {
                        Proveedor = c.RucReceptor,
                        Comprobante = c.IdComprobante,
                        Nombre = c.RazonSocialReceptor,
                        FechaDistribucion = fechaDist,
                        Factura = $"{c.Serie}-{c.Numero}",
                        FechaFactura = c.FechaEmision.ToString("d/MM/yyyy"),
                        TasaCambio = c.TasaCambio,
                        Moneda = c.Moneda,
                        ImpoCompra = c.MontoNeto,
                        IGV = c.MontoIGVCredito,
                        MntoFactura = c.MontoBruto,
                        TotalDistribucion = totalDist,
                        NroProveedor = string.Empty,
                        NombreProv = string.Empty,
                        NumRegFiscal = string.Empty,
                        SecDist = baseSeq + 10,
                        Proyecto = imp.Proyecto ?? string.Empty,
                        SistImpst = "1",
                        CodImp = "NR",
                        DescCodImp = "No Aplica Retención",
                        BaseImp = c.MontoNeto,
                        Importe = 0,
                        CuentaContable = imp.CuentaContable ?? string.Empty,
                        DescripcionCuenta = imp.DescripcionCuenta ?? string.Empty,
                        CodUnidad1 = imp.CodUnidad1Cuenta ?? string.Empty,
                        CodUnidad3 = imp.CodUnidad3Cuenta ?? string.Empty,
                        CodUnidad4 = imp.CodUnidad4Cuenta ?? string.Empty,
                        EsLineaPrincipal = false
                    });

                    // Subfila C — IGV18
                    resultado.Add(new SytelineDistribucionDto
                    {
                        Proveedor = c.RucReceptor,
                        Comprobante = c.IdComprobante,
                        Nombre = c.RazonSocialReceptor,
                        FechaDistribucion = fechaDist,
                        Factura = $"{c.Serie}-{c.Numero}",
                        FechaFactura = c.FechaEmision.ToString("d/MM/yyyy"),
                        TasaCambio = c.TasaCambio,
                        Moneda = c.Moneda,
                        ImpoCompra = c.MontoNeto,
                        IGV = c.MontoIGVCredito,
                        MntoFactura = c.MontoBruto,
                        TotalDistribucion = totalDist,
                        NroProveedor = string.Empty,
                        NombreProv = string.Empty,
                        NumRegFiscal = string.Empty,
                        SecDist = baseSeq + 15,
                        Proyecto = imp.Proyecto ?? string.Empty,
                        SistImpst = "2",
                        CodImp = "IGV18",
                        DescCodImp = "IGV 18%",
                        BaseImp = c.MontoNeto,
                        Importe = c.MontoIGVCredito,
                        CuentaContable = imp.CuentaContable ?? string.Empty,
                        DescripcionCuenta = imp.DescripcionCuenta ?? string.Empty,
                        CodUnidad1 = imp.CodUnidad1Cuenta ?? string.Empty,
                        CodUnidad3 = imp.CodUnidad3Cuenta ?? string.Empty,
                        CodUnidad4 = imp.CodUnidad4Cuenta ?? string.Empty,
                        EsLineaPrincipal = false
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
                    Proveedor = c.RucReceptor,
                    Nombre = c.RazonSocialReceptor,
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
                    TotalDetLocal = c.MontoDetraccion
                });
            }

            return result;
        }
        #endregion
    }
}