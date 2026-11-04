using Application.Abstractions.Gateways;
using Application.UseCases.Cliente.Queries.ConsultarClientePorDocumento;
using Domain.Entities;
using Domain.ValueObjects;
using Moq;

namespace UnitTests.Application.UseCases.Cliente.Queries.ConsultarClientePorDocumento;

public class ConsultarClientePorDocumentoQueryHandlerTests
{
    private readonly Mock<IClienteGateway> _mockGateway = new();

    [Fact]
    public async Task Handle_ReturnsTrue_WhenClienteExists()
    {
        // Arrange
        _mockGateway.Setup(g => g.GetByDocumentoAsync(It.IsAny<Documento>()))
            .ReturnsAsync(new ClienteAggregateRoot
            {
                Id = 1,
                Nome = "Cliente",
                TipoDocumento = TipoDocumento.Cpf,
                Documento = Documento.Criar("11144477735")
            });
        var handler = new ConsultarClientePorDocumentoQueryHandler(_mockGateway.Object);
        var query = new ConsultarClientePorDocumentoQuery { Documento = "11144477735" };

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public async Task Handle_ReturnsFalse_WhenClienteDoesNotExist()
    {
        // Arrange
        _mockGateway.Setup(g => g.GetByDocumentoAsync(It.IsAny<Documento>()))
            .ReturnsAsync((ClienteAggregateRoot?)null);
        var handler = new ConsultarClientePorDocumentoQueryHandler(_mockGateway.Object);
        var query = new ConsultarClientePorDocumentoQuery { Documento = "99999999999" };

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.False(result);
    }
}
