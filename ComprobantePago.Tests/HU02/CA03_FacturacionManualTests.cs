using ComprobantePago.Application.Commands.Comprobante;
using ComprobantePago.Application.DTOs.Comprobante.Requests;
using ComprobantePago.Application.Interfaces;
using ComprobantePago.Application.Interfaces.Services;
using ComprobantePago.Infrastructure.Repositories;
using ComprobantePago.Infrastructure.Services;
using ComprobantePago.Tests.Helpers;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;

namespace ComprobantePago.Tests.HU02
{
    /// <summary>
    /// CA-03: Permitir el ingreso de información manualmente para recibos
    /// (agua, luz, teléfono, vales de caja, vales de movilidad) y para
    /// la cabecera y detalle de montos.
    ///
    /// Verifica que GuardarAsync procese correctamente comprobantes ingresados
    /// manualmente sin necesidad de validación XML/SUNAT.
    /// </summary>
    public class CA03_FacturacionManualTests
    {
        private ComprobanteRepository ConstruirRepository(string dbNombre = "")
        {
            var db         = DbContextFactory.Crear(dbNombre.Length > 0 ? dbNombre : null);
            var unitOfWork = new Mock<IUnitOfWork>();
            unitOfWork.Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(0);
            unitOfWork.Setup(u => u.BeginTransactionAsync()).Returns(Task.CompletedTask);
            unitOfWork.Setup(u => u.CommitAsync()).Returns(Task.CompletedTask);

            var mockUsuario = new Mock<IUsuarioContexto>();
            mockUsuario.Setup(u => u.Correo).Returns("test@tecsur.com.pe");

            return new ComprobanteRepository(
                db, unitOfWork.Object,
                new XmlComprobanteService(),
                new PdfComprobanteService(),
                new Mock<ISunatService>().Object,
                mockUsuario.Object,
                NullLogger<ComprobanteRepository>.Instance);
        }

        private static RegistrarComprobanteCommand ComandoManual(
            string tipoDocumento,
            string serie,
            string numero,
            string descripcion = "") => new()
        {
            Comprobante = new RegistrarComprobanteDto
            {
                Folio           = string.Empty,   // sin folio → se genera automáticamente
                Ruc             = "10411309512",
                RazonSocial     = descripcion.Length > 0 ? descripcion : "PROVEEDOR MANUAL SAC",
                TipoDocumento   = tipoDocumento,
                TipoSunat       = "00",           // comprobantes manuales sin código SUNAT
                Serie           = serie,
                Numero          = numero,
                FechaEmision    = "01/04/2026",
                FechaRecepcion  = "02/04/2026",
                Moneda          = "PEN",
                TasaCambio      = 1m,
                MontoNeto       = 100.00m,
                MontoIGVCredito = 0m,
                MontoTotal      = 100.00m,
                MontoBruto      = 100.00m,
                TieneDetraccion = false
            }
        };

        // ── Generación de folio en modo manual ────────────────────────────────

        [Fact]
        public async Task GuardarManual_SinFolioPrevio_GeneraFolio()
        {
            var repo      = ConstruirRepository(nameof(GuardarManual_SinFolioPrevio_GeneraFolio));
            var resultado = await repo.GuardarAsync(ComandoManual("RP", "001", "00000001"));
            Assert.False(string.IsNullOrWhiteSpace(resultado),
                "Debe generarse un folio al guardar en modo manual.");
        }

        [Fact]
        public async Task GuardarManual_FolioTieneLongitudCorrecta()
        {
            var repo      = ConstruirRepository(nameof(GuardarManual_FolioTieneLongitudCorrecta));
            var resultado = await repo.GuardarAsync(ComandoManual("RP", "001", "00000001"));
            Assert.Equal(10, resultado.Length,
                "El folio generado debe tener formato YYYYMMNNNN (10 caracteres).");
        }

        // ── Tipos de comprobante manual permitidos ────────────────────────────

        [Theory]
        [InlineData("RP",  "001", "00000001", "Recibo de agua/luz/teléfono")]
        [InlineData("VC",  "001", "00000001", "Vale de caja")]
        [InlineData("VM",  "001", "00000001", "Vale de movilidad")]
        [InlineData("CI",  "001", "00000001", "Comprobante interno")]
        [InlineData("RH",  "001", "00000001", "Recibo por honorarios")]
        public async Task GuardarManual_AceptaTiposDeComprobanteInterno(
            string tipoDocumento, string serie, string numero, string descripcion)
        {
            var repo      = ConstruirRepository($"Manual_{tipoDocumento}");
            var resultado = await repo.GuardarAsync(
                ComandoManual(tipoDocumento, serie, numero, descripcion));
            Assert.False(string.IsNullOrWhiteSpace(resultado),
                $"{descripcion} ({tipoDocumento}) debe guardarse correctamente en modo manual.");
        }

        // ── Actualización de comprobante existente ────────────────────────────

        [Fact]
        public async Task GuardarManual_ConFolioExistente_ActualizaSinCrearDuplicado()
        {
            var dbNombre  = nameof(GuardarManual_ConFolioExistente_ActualizaSinCrearDuplicado);
            var repo      = ConstruirRepository(dbNombre);
            var db        = DbContextFactory.Crear(dbNombre);

            // Primer guardado → crea el registro
            var folio = await repo.GuardarAsync(ComandoManual("RP", "001", "00000001"));
            Assert.False(string.IsNullOrWhiteSpace(folio));

            // Segundo guardado con el mismo folio → actualiza
            var comando2 = ComandoManual("RP", "001", "00000001");
            comando2.Comprobante.Folio       = folio;
            comando2.Comprobante.MontoTotal  = 150.00m;
            comando2.Comprobante.MontoNeto   = 150.00m;
            comando2.Comprobante.MontoBruto  = 150.00m;

            var folioActualizado = await repo.GuardarAsync(comando2);
            Assert.Equal(folio, folioActualizado,
                "El folio no debe cambiar al actualizar un comprobante existente.");
        }

        // ── Cabecera completa en modo manual ──────────────────────────────────

        [Fact]
        public async Task GuardarManual_PersisteDatosDeMontos()
        {
            var dbNombre = nameof(GuardarManual_PersisteDatosDeMontos);
            var db       = DbContextFactory.Crear(dbNombre);

            var unitOfWork = new Mock<IUnitOfWork>();
            unitOfWork.Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(0);
            unitOfWork.Setup(u => u.BeginTransactionAsync()).Returns(Task.CompletedTask);
            unitOfWork.Setup(u => u.CommitAsync()).Returns(Task.CompletedTask);

            var mockUsuario = new Mock<IUsuarioContexto>();
            mockUsuario.Setup(u => u.Correo).Returns("test@tecsur.com.pe");

            var repo = new ComprobanteRepository(
                db, unitOfWork.Object,
                new XmlComprobanteService(), new PdfComprobanteService(),
                new Mock<ISunatService>().Object,
                mockUsuario.Object, NullLogger<ComprobanteRepository>.Instance);

            var comando = ComandoManual("RP", "001", "00000001");
            comando.Comprobante.MontoNeto       = 250.00m;
            comando.Comprobante.MontoIGVCredito = 45.00m;
            comando.Comprobante.MontoTotal      = 295.00m;
            comando.Comprobante.MontoBruto      = 295.00m;

            var folio = await repo.GuardarAsync(comando);

            var registrado = db.Comprobantes.FirstOrDefault(c => c.Folio == folio);
            Assert.NotNull(registrado);
            Assert.Equal(250.00m, registrado.MontoNeto);
            Assert.Equal(45.00m,  registrado.MontoIGVCredito);
            Assert.Equal(295.00m, registrado.MontoTotal);
        }

        [Fact]
        public async Task GuardarManual_RegistraUsuarioDigitacion()
        {
            var dbNombre = nameof(GuardarManual_RegistraUsuarioDigitacion);
            var db       = DbContextFactory.Crear(dbNombre);

            var unitOfWork = new Mock<IUnitOfWork>();
            unitOfWork.Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(0);
            unitOfWork.Setup(u => u.BeginTransactionAsync()).Returns(Task.CompletedTask);
            unitOfWork.Setup(u => u.CommitAsync()).Returns(Task.CompletedTask);

            var mockUsuario = new Mock<IUsuarioContexto>();
            mockUsuario.Setup(u => u.Correo).Returns("operador@tecsur.com.pe");

            var repo = new ComprobanteRepository(
                db, unitOfWork.Object,
                new XmlComprobanteService(), new PdfComprobanteService(),
                new Mock<ISunatService>().Object,
                mockUsuario.Object, NullLogger<ComprobanteRepository>.Instance);

            var folio = await repo.GuardarAsync(ComandoManual("RP", "001", "00000001"));

            var registrado = db.Comprobantes.FirstOrDefault(c => c.Folio == folio);
            Assert.NotNull(registrado);
            Assert.Equal("operador@tecsur.com.pe", registrado.RolDigitacion);
            Assert.NotNull(registrado.FechaDigitacion);
        }

        [Fact]
        public async Task GuardarManual_EstadoInicialEsRegistrado()
        {
            var dbNombre = nameof(GuardarManual_EstadoInicialEsRegistrado);
            var db       = DbContextFactory.Crear(dbNombre);

            var unitOfWork = new Mock<IUnitOfWork>();
            unitOfWork.Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(0);
            unitOfWork.Setup(u => u.BeginTransactionAsync()).Returns(Task.CompletedTask);
            unitOfWork.Setup(u => u.CommitAsync()).Returns(Task.CompletedTask);

            var mockUsuario = new Mock<IUsuarioContexto>();
            mockUsuario.Setup(u => u.Correo).Returns("test@tecsur.com.pe");

            var repo = new ComprobanteRepository(
                db, unitOfWork.Object,
                new XmlComprobanteService(), new PdfComprobanteService(),
                new Mock<ISunatService>().Object,
                mockUsuario.Object, NullLogger<ComprobanteRepository>.Instance);

            var folio      = await repo.GuardarAsync(ComandoManual("RP", "001", "00000001"));
            var registrado = db.Comprobantes.FirstOrDefault(c => c.Folio == folio);

            Assert.NotNull(registrado);
            Assert.Equal("REGISTRADO", registrado.CodigoEstado);
        }
    }
}
