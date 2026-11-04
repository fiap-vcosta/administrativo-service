using Domain.Entities;
using Domain.ValueObjects;

namespace UnitTests.Domain.Entities;

public class ClienteAggregateRootTests
{
    [Fact]
    public void CreateClienteAggregateRoot_SetsPropertiesCorrectly()
    {
        // Arrange
        var cliente = new ClienteAggregateRoot
        {
            Id = 42,
            Nome = "João Silva",
            Email = "joao@teste.com",
            TipoDocumento = TipoDocumento.Cpf,
            Documento = Documento.Criar("12345678901")
        };

        // Act & Assert
        Assert.Equal(42, cliente.Id);
        Assert.Equal("João Silva", cliente.Nome);
        Assert.Equal("joao@teste.com", cliente.Email);
        Assert.Equal(TipoDocumento.Cpf, cliente.TipoDocumento);
        Assert.Equal("12345678901", cliente.Documento.Valor);
    }
}
