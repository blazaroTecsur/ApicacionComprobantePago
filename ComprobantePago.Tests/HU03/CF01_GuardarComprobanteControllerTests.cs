using ComprobantePago.Application.Commands.Comprobante;
using ComprobantePago.Application.DTOs.Comprobante.Requests;
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
    /// HU-03 · CF-01 — Pruebas funcionales sobre el endpoint POST /Comprobante/Guardar.
    /// Verifica que el controlador rechaza datos inválidos antes de llamar al repositorio
    /// y devuelve la respuesta correcta para datos válidos.
    /// </summary>
    public class CF01_GuardarComprobanteControllerTests
    {
        private static ComprobanteController ConstruirControlador(
            Mock<IComprobanteRepository>? repoMock = null)
        {
            repoMock ??= new Mock<IComprobanteRepository>();
            repoMock.Setup(r => r.GuardarAsync(It.IsAny<RegistrarComprobanteCommand>()))
                    .ReturnsAsync("2026040001");

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

        private static RegistrarComprobanteCommand ComandoValido() => new()
        {
            Comprobante = new RegistrarComprobanteDto
            {
                Ruc             = "20206018411",
                RazonSocial     = "EMPRESA PROVEEDORA SAC",
                TipoDocumento   = "FP",
                TipoSunat       = "01",
                Serie           = "F001",
                Numero          = "00000100",
                FechaEmision    = "01/04/2026",
                Moneda          = "PEN",
                TasaCambio      = 1m,
                MontoTotal      = 1180m,
                MontoNeto       = 1000m,
                MontoIGVCredito = 180m,
                TieneDetraccion = false
            }
        };

        // ── Respuesta exitosa ─────────────────────────────────────────────────

        [Fact]
        public async Task Guardar_DatosValidos_Devuelve200ConFolio()
        {
            var controller = ConstruirControlador();
            var result     = await controller.Guardar(ComandoValido()) as OkObjectResult;

            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);

            var valor = result.Value!;
            var exito = (bool)valor.GetType().GetProperty("exito")!.GetValue(valor)!;
            var folio = (string)valor.GetType().GetProperty("folio")!.GetValue(valor)!;

            Assert.True(exito);
            Assert.Equal("2026040001", folio);
        }

        [Fact]
        public async Task Guardar_DatosValidos_LlamaAlRepositorio()
        {
            var repoMock = new Mock<IComprobanteRepository>();
            repoMock.Setup(r => r.GuardarAsync(It.IsAny<RegistrarComprobanteCommand>()))
                    .ReturnsAsync("2026040001");

            var controller = ConstruirControlador(repoMock);
            await controller.Guardar(ComandoValido());

            repoMock.Verify(r => r.GuardarAsync(It.IsAny<RegistrarComprobanteCommand>()),
                Times.Once);
        }

        // ── Validación de montos ──────────────────────────────────────────────

        [Fact]
        public async Task Guardar_MontoTotalCero_Devuelve400()
        {
            var cmd = ComandoValido();
            cmd.Comprobante.MontoTotal = 0;

            var result = await ConstruirControlador().Guardar(cmd) as BadRequestObjectResult;

            Assert.NotNull(result);
            Assert.Equal(400, result.StatusCode);
        }

        [Fact]
        public async Task Guardar_MontoTotalCero_NoLlamaAlRepositorio()
        {
            var repoMock = new Mock<IComprobanteRepository>();
            var controller = ConstruirControlador(repoMock);

            var cmd = ComandoValido();
            cmd.Comprobante.MontoTotal = 0;
            await controller.Guardar(cmd);

            repoMock.Verify(r => r.GuardarAsync(It.IsAny<RegistrarComprobanteCommand>()),
                Times.Never);
        }

        [Fact]
        public async Task Guardar_MontoTotalCero_MensajeValidacionCorrecto()
        {
            var cmd = ComandoValido();
            cmd.Comprobante.MontoTotal = 0;

            var result  = await ConstruirControlador().Guardar(cmd) as BadRequestObjectResult;
            var mensaje = result!.Value!.GetType().GetProperty("error")!.GetValue(result.Value)!;
            var user    = mensaje.GetType().GetProperty("userMessage")!.GetValue(mensaje)!.ToString();

            Assert.Contains("monto total", user, StringComparison.OrdinalIgnoreCase);
        }

        // ── Validación de RUC ─────────────────────────────────────────────────

        [Fact]
        public async Task Guardar_RucVacio_Devuelve400()
        {
            var cmd = ComandoValido();
            cmd.Comprobante.Ruc = string.Empty;

            var result = await ConstruirControlador().Guardar(cmd) as BadRequestObjectResult;
            Assert.NotNull(result);
            Assert.Equal(400, result.StatusCode);
        }

        [Fact]
        public async Task Guardar_RucConMenosDe11Digitos_Devuelve400()
        {
            var cmd = ComandoValido();
            cmd.Comprobante.Ruc = "2020601841";  // 10 dígitos

            var result = await ConstruirControlador().Guardar(cmd) as BadRequestObjectResult;
            Assert.NotNull(result);
        }

        // ── Validación de detracción ──────────────────────────────────────────

        [Fact]
        public async Task Guardar_ConDetraccionSinTipo_Devuelve400()
        {
            var cmd = ComandoValido();
            cmd.Comprobante.TieneDetraccion = true;
            cmd.Comprobante.TipoDetraccion  = string.Empty;
            cmd.Comprobante.PorcentajeDetraccion = 4m;
            cmd.Comprobante.MontoDetraccion = 47.2m;

            var result = await ConstruirControlador().Guardar(cmd) as BadRequestObjectResult;
            Assert.NotNull(result);
            Assert.Equal(400, result.StatusCode);
        }

        [Fact]
        public async Task Guardar_ConDetraccionPorcentajeSuperior100_Devuelve400()
        {
            var cmd = ComandoValido();
            cmd.Comprobante.TieneDetraccion      = true;
            cmd.Comprobante.TipoDetraccion       = "030";
            cmd.Comprobante.PorcentajeDetraccion = 110m;
            cmd.Comprobante.MontoDetraccion      = 100m;

            var result = await ConstruirControlador().Guardar(cmd) as BadRequestObjectResult;
            Assert.NotNull(result);
        }

        [Fact]
        public async Task Guardar_ConDetraccionCompleta_Devuelve200()
        {
            var cmd = ComandoValido();
            cmd.Comprobante.TieneDetraccion      = true;
            cmd.Comprobante.TipoDetraccion       = "030";
            cmd.Comprobante.PorcentajeDetraccion = 4m;
            cmd.Comprobante.MontoDetraccion      = 47.2m;

            var result = await ConstruirControlador().Guardar(cmd) as OkObjectResult;
            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);
        }
    }
}
