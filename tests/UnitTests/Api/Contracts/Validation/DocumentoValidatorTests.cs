using Api.Contracts.Validation;

namespace UnitTests.Api.Contracts.Validation;

public class DocumentoValidatorTests
{
    [Fact]
    public void TryNormalizeValidCpfOrCnpj_ReturnsTrue_ForValidCpf()
    {
        // Arrange / Act
        var ok = DocumentoValidator.TryNormalizeValidCpfOrCnpj(
            "111.444.777-35",
            out var normalized,
            out var errors);

        // Assert
        Assert.True(ok);
        Assert.Equal("11144477735", normalized);
        Assert.Empty(errors);
    }

    [Fact]
    public void TryNormalizeValidCpfOrCnpj_ReturnsFalse_ForInvalidDocumento()
    {
        // Arrange / Act
        var ok = DocumentoValidator.TryNormalizeValidCpfOrCnpj("123", out _, out var errors);

        // Assert
        Assert.False(ok);
        Assert.Contains(errors, e => e.Contains("inválido", StringComparison.OrdinalIgnoreCase));
    }

    [Fact]
    public void TryNormalizeValidCpfOrCnpj_ReturnsFalse_WhenEmpty()
    {
        // Arrange / Act
        var ok = DocumentoValidator.TryNormalizeValidCpfOrCnpj("  ", out _, out var errors);

        // Assert
        Assert.False(ok);
        Assert.Contains(errors, e => e.Contains("obrigatório", StringComparison.OrdinalIgnoreCase));
    }

    [Fact]
    public void TryNormalizeValidCpfOrCnpj_ReturnsTrue_ForValidCnpj()
    {
        // Arrange / Act
        var ok = DocumentoValidator.TryNormalizeValidCpfOrCnpj(
            "04.252.011/0001-10",
            out var normalized,
            out var errors);

        // Assert
        Assert.True(ok);
        Assert.Equal("04252011000110", normalized);
        Assert.Empty(errors);
    }
}
