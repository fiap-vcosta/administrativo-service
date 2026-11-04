using Domain.Exceptions;
using Domain.ValueObjects;

namespace UnitTests.Domain.ValueObjects;

public class DocumentoTests
{
    [Fact]
    public void Criar_NormalizesFormattingCharacters()
    {
        // Arrange / Act
        var documento = Documento.Criar("111.444.777-35");

        // Assert
        Assert.Equal("11144477735", documento.Valor);
        Assert.Equal("11144477735", documento.ToString());
    }

    [Fact]
    public void Criar_Throws_WhenEmpty()
    {
        // Arrange / Act / Assert
        Assert.Throws<BusinessRuleException>(() => Documento.Criar("   "));
    }

    [Fact]
    public void Criar_Throws_WhenOnlySeparators()
    {
        // Arrange / Act / Assert
        Assert.Throws<BusinessRuleException>(() => Documento.Criar("...---"));
    }
}
