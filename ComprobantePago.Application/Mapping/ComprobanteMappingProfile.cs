using AutoMapper;
using ComprobantePago.Application.DTOs.Comprobante.Requests;
using ComprobantePago.Application.DTOs.Comprobante.Response;
using ComprobantePago.Domain.Entities;

namespace ComprobantePago.Application.Mapping
{
    public class ComprobanteMappingProfile : Profile
    {
        public ComprobanteMappingProfile()
        {
            // ── Comprobante ──────────────────────────────────────────────────

            // Entidad → DTO resumen (listado)
            CreateMap<Comprobante, ComprobanteDto>()
                .ForMember(d => d.TipoComprobante,
                    opt => opt.MapFrom(s => s.TipoDocumento))
                .ForMember(d => d.Proveedor,
                    opt => opt.MapFrom(s => s.RazonSocialReceptor))
                .ForMember(d => d.Fecha,
                    opt => opt.MapFrom(s => s.FechaEmision.ToString("dd/MM/yyyy")))
                .ForMember(d => d.Estado,
                    opt => opt.MapFrom(s => s.CodigoEstado));

            // ── Imputación ───────────────────────────────────────────────────

            // Entidad → DTO detalle
            CreateMap<ImputacionContable, ImputacionDetalleDto>()
                .ForMember(d => d.AliasCuenta,
                    opt => opt.MapFrom(s => s.AliasCuenta ?? string.Empty))
                .ForMember(d => d.CuentaContable,
                    opt => opt.MapFrom(s => s.CuentaContable ?? string.Empty))
                .ForMember(d => d.DescripcionCuenta,
                    opt => opt.MapFrom(s => s.DescripcionCuenta ?? string.Empty))
                .ForMember(d => d.Descripcion,
                    opt => opt.MapFrom(s => s.Descripcion ?? string.Empty))
                .ForMember(d => d.Proyecto,
                    opt => opt.MapFrom(s => s.Proyecto ?? string.Empty))
                .ForMember(d => d.CodUnidad1Cuenta,
                    opt => opt.MapFrom(s => s.CodUnidad1Cuenta ?? string.Empty))
                .ForMember(d => d.CodUnidad3Cuenta,
                    opt => opt.MapFrom(s => s.CodUnidad3Cuenta ?? string.Empty))
                .ForMember(d => d.CodUnidad4Cuenta,
                    opt => opt.MapFrom(s => s.CodUnidad4Cuenta ?? string.Empty));

            // DTO request → Entidad (crear / actualizar)
            CreateMap<ImputacionDto, ImputacionContable>()
                .ForMember(d => d.Secuencia,
                    opt => opt.MapFrom(s => ParseInt(s.Secuencia)))
                .ForMember(d => d.IdImputacionContable, opt => opt.Ignore())
                .ForMember(d => d.UsuarioReg, opt => opt.Ignore())
                .ForMember(d => d.FechaReg, opt => opt.Ignore())
                .ForMember(d => d.UsuarioAct, opt => opt.Ignore())
                .ForMember(d => d.FechaAct, opt => opt.Ignore())
                .ForMember(d => d.Comprobante, opt => opt.Ignore());
        }

        private static int ParseInt(string value) =>
            int.TryParse(value, out var n) ? n : 0;
    }
}
