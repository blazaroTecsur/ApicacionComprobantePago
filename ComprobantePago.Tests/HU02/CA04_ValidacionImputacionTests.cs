using ComprobantePago.Application.DTOs.Comprobante.Requests;
using ComprobantePago.Application.Validations;

namespace ComprobantePago.Tests.HU02
{
    /// <summary>
    /// CA-04: Validar que las imputaciones contables requieren todos los
    /// códigos de unidad antes de ser registradas.
    /// </summary>
    public class CA04_ValidacionImputacionTests
    {
        private readonly ImputacionValidator _validator = new();

        private static ImputacionDto DtoValido() => new()
        {
            Folio             = "2026040001",
            CuentaContable    = "6201001",
            DescripcionCuenta = "Remuneraciones",
            CodUnidad1Cuenta  = "U1-001",
            CodUnidad3Cuenta  = "U3-010",
            CodUnidad4Cuenta  = "U4-050",
            Monto             = 500m,
            Descripcion       = "Pago servicios"
        };

        // ── Folio ─────────────────────────────────────────────────────────────

        [Fact]
        public void Validar_FolioVacio_Falla()
        {
            var dto = DtoValido(); dto.Folio = string.Empty;
            var result = _validator.Validate(dto);
            Assert.False(result.IsValid);
            Assert.Contains(result.Errors, e => e.PropertyName == nameof(dto.Folio));
        }

        // ── Cuenta Contable ───────────────────────────────────────────────────

        [Fact]
        public void Validar_CuentaContableVacia_Falla()
        {
            var dto = DtoValido(); dto.CuentaContable = string.Empty;
            var result = _validator.Validate(dto);
            Assert.False(result.IsValid);
            Assert.Contains(result.Errors, e => e.PropertyName == nameof(dto.CuentaContable));
        }

        // ── Descripción ───────────────────────────────────────────────────────

        [Fact]
        public void Validar_DescripcionCuentaVacia_Falla()
        {
            var dto = DtoValido(); dto.DescripcionCuenta = string.Empty;
            var result = _validator.Validate(dto);
            Assert.False(result.IsValid);
            Assert.Contains(result.Errors, e => e.PropertyName == nameof(dto.DescripcionCuenta));
        }

        [Fact]
        public void Validar_DescripcionCuentaSuperaMaximo_Falla()
        {
            var dto = DtoValido(); dto.DescripcionCuenta = new string('X', 201);
            var result = _validator.Validate(dto);
            Assert.False(result.IsValid);
            Assert.Contains(result.Errors, e => e.PropertyName == nameof(dto.DescripcionCuenta));
        }

        // ── Códigos de Unidad ─────────────────────────────────────────────────

        [Fact]
        public void Validar_CodUnidad1Vacio_Falla()
        {
            var dto = DtoValido(); dto.CodUnidad1Cuenta = string.Empty;
            var result = _validator.Validate(dto);
            Assert.False(result.IsValid);
            Assert.Contains(result.Errors, e => e.PropertyName == nameof(dto.CodUnidad1Cuenta));
        }

        [Fact]
        public void Validar_CodUnidad3Vacio_Falla()
        {
            var dto = DtoValido(); dto.CodUnidad3Cuenta = string.Empty;
            var result = _validator.Validate(dto);
            Assert.False(result.IsValid);
            Assert.Contains(result.Errors, e => e.PropertyName == nameof(dto.CodUnidad3Cuenta));
        }

        [Fact]
        public void Validar_CodUnidad4Vacio_Falla()
        {
            var dto = DtoValido(); dto.CodUnidad4Cuenta = string.Empty;
            var result = _validator.Validate(dto);
            Assert.False(result.IsValid);
            Assert.Contains(result.Errors, e => e.PropertyName == nameof(dto.CodUnidad4Cuenta));
        }

        [Theory]
        [InlineData("CodUnidad1Cuenta", "El código de unidad 1 es obligatorio.")]
        [InlineData("CodUnidad3Cuenta", "El código de unidad 3 es obligatorio.")]
        [InlineData("CodUnidad4Cuenta", "El código de unidad 4 es obligatorio.")]
        public void Validar_CodUnidadVacio_MensajeEsCorrecto(string campo, string mensajeEsperado)
        {
            var dto = DtoValido();
            typeof(ImputacionDto).GetProperty(campo)!.SetValue(dto, string.Empty);
            var result = _validator.Validate(dto);
            Assert.Contains(result.Errors, e => e.ErrorMessage == mensajeEsperado);
        }

        // ── DTO completo válido ───────────────────────────────────────────────

        [Fact]
        public void Validar_DtoCompletoYValido_Pasa()
        {
            var result = _validator.Validate(DtoValido());
            Assert.True(result.IsValid,
                $"DTO válido no debería tener errores. Errores: {string.Join(", ", result.Errors.Select(e => e.ErrorMessage))}");
        }
    }
}
