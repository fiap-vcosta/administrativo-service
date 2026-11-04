using Domain.Exceptions;

namespace UnitTests.Domain.Exceptions;

public class DomainExceptionTests
{
    [Fact]
    public void BusinessRuleException_SetsMessage()
    {
        // Arrange / Act
        var ex = new BusinessRuleException("regra");

        // Assert
        Assert.Equal("regra", ex.Message);
    }

    [Fact]
    public void DomainNotFoundException_SetsMessage()
    {
        // Arrange / Act
        var ex = new DomainNotFoundException("nao achou");

        // Assert
        Assert.Equal("nao achou", ex.Message);
    }

    [Fact]
    public void DomainException_SetsMessage()
    {
        // Arrange / Act
        var ex = new DomainException("dominio");

        // Assert
        Assert.Equal("dominio", ex.Message);
    }
}
