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
    /// HU-03 · CF-03 — Pruebas funcionales sobre los endpoints
    /// POST /Comprobante/EditarImputacion y POST /Comprobante/EliminarImputacion.
    /// Verifica respuestas HTTP correctas y que la validación bloquea datos inválidos.
    /// </summary>
    public class CF03_EditarImputacionControllerTests
    {
        private static readonly ImputacionDetalleDto _imputacionFalsa = new()
        {
            Secuencia         = 1,
            Folio             = "2026040001",
            CuentaContable    = "6201001",
            DescripcionCuenta = "Remuneraciones",
            Monto             = 750m,
            CodUnidad1Cuenta  = "U1-002"
        };

        private static ComprobanteController ConstruirControlador(
            Mock<IComprobanteRepository>? repoMock = null)
        {
            repoMock ??= new Mock<IComprobanteRepository>();
            repoMock.Setup(r => r.EditarImputacionAsync(It.IsAny<EditarImputacionCommand>()))
                    .ReturnsAsync(_imputacionFalsa);
            repoMock.Setup(r => r.EliminarImputacionAsync(It.IsAny<EliminarImputacionCommand>()))
                    .Returns(Task.CompletedTask);

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

        private static ImputacionDto DtoEdicionValido() => new()
        {
            Folio             = "2026040001",
            Secuencia         = "1",
            CuentaContable    = "6201001",
            DescripcionCuenta = "Remuneraciones actualizadas",
            Monto             = 750m,
            Descripcion       = "Descripción editada",
            CodUnidad1Cuenta  = "U1-002",
            CodUnidad3Cuenta  = string.Empty,
            CodUnidad4Cuenta  = string.Empty
        };

        // ── Editar: respuesta exitosa ─────────────────────────────────────────

        [Fact]
        public async Task EditarImputacion_DatosValidos_Devuelve200ConExito()
        {
            var result = await ConstruirControlador()
                .EditarImputacion(new EditarImputacionCommand { Imputacion = DtoEdicionValido() })
                as OkObjectResult;

            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);

            var exito = (bool)result.Value!.GetType().GetProperty("exito")!.GetValue(result.Value)!;
            Assert.True(exito);
        }

        [Fact]
        public async Task EditarImputacion_CambiaSoloCodUnidad4_Devuelve200()
        {
            var dto = DtoEdicionValido();
            dto.CodUnidad1Cuenta = string.Empty;
            dto.CodUnidad4Cuenta = "U4-099";

            var result = await ConstruirControlador()
                .EditarImputacion(new EditarImputacionCommand { Imputacion = dto })
                as OkObjectResult;

            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);
        }

        // ── Editar: validación ────────────────────────────────────────────────

        [Fact]
        public async Task EditarImputacion_SinCuentaContable_Devuelve400()
        {
            var dto = DtoEdicionValido();
            dto.CuentaContable = string.Empty;

            var result = await ConstruirControlador()
                .EditarImputacion(new EditarImputacionCommand { Imputacion = dto })
                as BadRequestObjectResult;

            Assert.NotNull(result);
            Assert.Equal(400, result.StatusCode);
        }

        [Fact]
        public async Task EditarImputacion_SinNingunCodUnidad_Devuelve400()
        {
            var dto = DtoEdicionValido();
            dto.CodUnidad1Cuenta = string.Empty;
            dto.CodUnidad3Cuenta = string.Empty;
            dto.CodUnidad4Cuenta = string.Empty;

            var result = await ConstruirControlador()
                .EditarImputacion(new EditarImputacionCommand { Imputacion = dto })
                as BadRequestObjectResult;

            Assert.NotNull(result);
            Assert.Equal(400, result.StatusCode);
        }

        [Fact]
        public async Task EditarImputacion_SinNingunCodUnidad_NoLlamaAlRepositorio()
        {
            var repoMock   = new Mock<IComprobanteRepository>();
            var controller = ConstruirControlador(repoMock);

            var dto = DtoEdicionValido();
            dto.CodUnidad1Cuenta = dto.CodUnidad3Cuenta = dto.CodUnidad4Cuenta = string.Empty;
            await controller.EditarImputacion(new EditarImputacionCommand { Imputacion = dto });

            repoMock.Verify(r => r.EditarImputacionAsync(It.IsAny<EditarImputacionCommand>()),
                Times.Never);
        }

        [Fact]
        public async Task EditarImputacion_DatosValidos_LlamaAlRepositorioUnaVez()
        {
            var repoMock = new Mock<IComprobanteRepository>();
            repoMock.Setup(r => r.EditarImputacionAsync(It.IsAny<EditarImputacionCommand>()))
                    .ReturnsAsync(_imputacionFalsa);

            var controller = ConstruirControlador(repoMock);
            await controller.EditarImputacion(
                new EditarImputacionCommand { Imputacion = DtoEdicionValido() });

            repoMock.Verify(r => r.EditarImputacionAsync(It.IsAny<EditarImputacionCommand>()),
                Times.Once);
        }

        // ── Eliminar ──────────────────────────────────────────────────────────

        [Fact]
        public async Task EliminarImputacion_Devuelve200()
        {
            var result = await ConstruirControlador()
                .EliminarImputacion(new EliminarImputacionCommand
                {
                    Folio     = "2026040001",
                    Secuencia = "1"
                })
                as OkObjectResult;

            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);
        }

        [Fact]
        public async Task EliminarImputacion_LlamaAlRepositorioUnaVez()
        {
            var repoMock = new Mock<IComprobanteRepository>();
            repoMock.Setup(r => r.EliminarImputacionAsync(It.IsAny<EliminarImputacionCommand>()))
                    .Returns(Task.CompletedTask);

            var controller = ConstruirControlador(repoMock);
            await controller.EliminarImputacion(new EliminarImputacionCommand
            {
                Folio     = "2026040001",
                Secuencia = "1"
            });

            repoMock.Verify(r => r.EliminarImputacionAsync(It.IsAny<EliminarImputacionCommand>()),
                Times.Once);
        }
    }
}
