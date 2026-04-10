using ComprobantePago.Application.Commands.Imputacion;
using ComprobantePago.Application.DTOs.Comprobante.Requests;
using ComprobantePago.Application.DTOs.Comprobante.Response;
using ComprobantePago.Application.Interfaces.QueryServices;
using ComprobantePago.Application.Interfaces.Repositories;
using ComprobantePago.Application.Interfaces.Services;
using ComprobantePago.Application.Interfaces.Services.Maestros;
using ComprobantePago.Application.Validations;
using ComprobantePago.Web.Controllers;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace ComprobantePago.Tests.HU03
{
    /// <summary>
    /// HU-03 · CF-02 — Pruebas funcionales sobre el endpoint POST /Comprobante/AgregarImputacion.
    /// Verifica que el controlador valida los códigos de unidad y la cuenta contable
    /// antes de persistir, y devuelve la respuesta correcta.
    /// </summary>
    public class CF02_AgregarImputacionControllerTests
    {
        private static readonly ImputacionDetalleDto _imputacionFalsa = new()
        {
            Secuencia         = 1,
            Folio             = "2026040001",
            CuentaContable    = "6201001",
            DescripcionCuenta = "Remuneraciones",
            Monto             = 500m,
            CodUnidad1Cuenta  = "U1-001"
        };

        private static ComprobanteController ConstruirControlador(
            Mock<IComprobanteRepository>? repoMock = null)
        {
            repoMock ??= new Mock<IComprobanteRepository>();
            repoMock.Setup(r => r.AgregarImputacionAsync(It.IsAny<AgregarImputacionCommand>()))
                    .ReturnsAsync(_imputacionFalsa);

            return new ComprobanteController(
                new Mock<IComprobanteQueryService>().Object,
                new Mock<ISytelineQueryService>().Object,
                new Mock<IMaestrosQueryService>().Object,
                repoMock.Object,
                new Mock<IExcelSytelineService>().Object,
                new Mock<IProveedorService>().Object,
                new RegistrarComprobanteValidator(),
                new ImputacionValidator());
        }

        private static ImputacionDto DtoValido() => new()
        {
            Folio             = "2026040001",
            CuentaContable    = "6201001",
            DescripcionCuenta = "Remuneraciones básicas",
            Monto             = 500m,
            Descripcion       = "Servicio consultoría",
            CodUnidad1Cuenta  = "U1-001",
            CodUnidad3Cuenta  = string.Empty,
            CodUnidad4Cuenta  = string.Empty
        };

        // ── Respuesta exitosa ─────────────────────────────────────────────────

        [Fact]
        public async Task AgregarImputacion_DatosValidos_Devuelve200ConExito()
        {
            var result = await ConstruirControlador()
                .AgregarImputacion(new AgregarImputacionCommand { Imputacion = DtoValido() })
                as OkObjectResult;

            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);

            var exito = (bool)result.Value!.GetType().GetProperty("exito")!.GetValue(result.Value)!;
            Assert.True(exito);
        }

        [Fact]
        public async Task AgregarImputacion_SoloCodUnidad3_Devuelve200()
        {
            var dto = DtoValido();
            dto.CodUnidad1Cuenta = string.Empty;
            dto.CodUnidad3Cuenta = "U3-010";

            var result = await ConstruirControlador()
                .AgregarImputacion(new AgregarImputacionCommand { Imputacion = dto })
                as OkObjectResult;

            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);
        }

        [Fact]
        public async Task AgregarImputacion_SoloCodUnidad4_Devuelve200()
        {
            var dto = DtoValido();
            dto.CodUnidad1Cuenta = string.Empty;
            dto.CodUnidad4Cuenta = "U4-050";

            var result = await ConstruirControlador()
                .AgregarImputacion(new AgregarImputacionCommand { Imputacion = dto })
                as OkObjectResult;

            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);
        }

        // ── Validación cuenta contable ────────────────────────────────────────

        [Fact]
        public async Task AgregarImputacion_SinCuentaContable_Devuelve400()
        {
            var dto = DtoValido();
            dto.CuentaContable = string.Empty;

            var result = await ConstruirControlador()
                .AgregarImputacion(new AgregarImputacionCommand { Imputacion = dto })
                as BadRequestObjectResult;

            Assert.NotNull(result);
            Assert.Equal(400, result.StatusCode);
        }

        [Fact]
        public async Task AgregarImputacion_SinDescripcionCuenta_Devuelve400()
        {
            var dto = DtoValido();
            dto.DescripcionCuenta = string.Empty;

            var result = await ConstruirControlador()
                .AgregarImputacion(new AgregarImputacionCommand { Imputacion = dto })
                as BadRequestObjectResult;

            Assert.NotNull(result);
        }

        // ── Validación códigos de unidad ──────────────────────────────────────

        [Fact]
        public async Task AgregarImputacion_SinNingunCodUnidad_Devuelve400()
        {
            var dto = DtoValido();
            dto.CodUnidad1Cuenta = string.Empty;
            dto.CodUnidad3Cuenta = string.Empty;
            dto.CodUnidad4Cuenta = string.Empty;

            var result = await ConstruirControlador()
                .AgregarImputacion(new AgregarImputacionCommand { Imputacion = dto })
                as BadRequestObjectResult;

            Assert.NotNull(result);
            Assert.Equal(400, result.StatusCode);
        }

        [Fact]
        public async Task AgregarImputacion_SinNingunCodUnidad_MensajeIndicaAlMenosUno()
        {
            var dto = DtoValido();
            dto.CodUnidad1Cuenta = string.Empty;
            dto.CodUnidad3Cuenta = string.Empty;
            dto.CodUnidad4Cuenta = string.Empty;

            var result  = await ConstruirControlador()
                .AgregarImputacion(new AgregarImputacionCommand { Imputacion = dto })
                as BadRequestObjectResult;

            var error   = result!.Value!.GetType().GetProperty("error")!.GetValue(result.Value)!;
            var mensaje = error.GetType().GetProperty("userMessage")!.GetValue(error)!.ToString();

            Assert.Contains("al menos un código de unidad", mensaje,
                StringComparison.OrdinalIgnoreCase);
        }

        [Fact]
        public async Task AgregarImputacion_SinNingunCodUnidad_NoLlamaAlRepositorio()
        {
            var repoMock = new Mock<IComprobanteRepository>();
            var controller = ConstruirControlador(repoMock);

            var dto = DtoValido();
            dto.CodUnidad1Cuenta = dto.CodUnidad3Cuenta = dto.CodUnidad4Cuenta = string.Empty;
            await controller.AgregarImputacion(new AgregarImputacionCommand { Imputacion = dto });

            repoMock.Verify(r => r.AgregarImputacionAsync(It.IsAny<AgregarImputacionCommand>()),
                Times.Never);
        }

        // ── Validación folio ──────────────────────────────────────────────────

        [Fact]
        public async Task AgregarImputacion_SinFolio_Devuelve400()
        {
            var dto = DtoValido();
            dto.Folio = string.Empty;

            var result = await ConstruirControlador()
                .AgregarImputacion(new AgregarImputacionCommand { Imputacion = dto })
                as BadRequestObjectResult;

            Assert.NotNull(result);
        }
    }
}
