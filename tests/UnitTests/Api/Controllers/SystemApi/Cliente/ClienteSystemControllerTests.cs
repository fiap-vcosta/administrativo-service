using Api.Controllers.SystemApi.Cliente;
using Application.UseCases.Cliente.Queries.ConsultarClientePorDocumento;
using Application.UseCases.Cliente.Queries.GetClienteById;
using Application.UseCases.Cliente.Responses;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace UnitTests.Api.Controllers.SystemApi.Cliente;

public class ClienteSystemControllerTests
{
    private readonly Mock<IMediator> _mediatorMock = new();

    [Fact]
    public async Task PorDocumento_ReturnsBadRequest_WhenDocumentoInvalid()
    {
        // Arrange
        var controller = new ClienteSystemController(_mediatorMock.Object);

        // Act
        var result = await controller.PorDocumento("123", CancellationToken.None);

        // Assert
        Assert.IsType<BadRequestObjectResult>(result);
        _mediatorMock.Verify(
            m => m.Send(It.IsAny<ConsultarClientePorDocumentoQuery>(), It.IsAny<CancellationToken>()),
            Times.Never);
    }

    [Fact]
    public async Task PorDocumento_ReturnsOk_WhenClienteExists()
    {
        // Arrange
        _mediatorMock
            .Setup(m => m.Send(It.IsAny<ConsultarClientePorDocumentoQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);
        var controller = new ClienteSystemController(_mediatorMock.Object);

        // Act
        var result = await controller.PorDocumento("11144477735", CancellationToken.None);

        // Assert
        Assert.IsType<OkResult>(result);
    }

    [Fact]
    public async Task PorDocumento_ReturnsNotFound_WhenClienteDoesNotExist()
    {
        // Arrange
        _mediatorMock
            .Setup(m => m.Send(It.IsAny<ConsultarClientePorDocumentoQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);
        var controller = new ClienteSystemController(_mediatorMock.Object);

        // Act
        var result = await controller.PorDocumento("11144477735", CancellationToken.None);

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task GetById_ReturnsOk_WhenFound()
    {
        // Arrange
        _mediatorMock
            .Setup(m => m.Send(It.IsAny<GetClienteByIdQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ClienteResponse
            {
                Id = 1,
                Nome = "Cliente",
                Email = "c@teste.com",
                TipoDocumento = TipoDocumento.Cpf,
                Documento = "11144477735"
            });
        var controller = new ClienteSystemController(_mediatorMock.Object);

        // Act
        var result = await controller.GetById(1, CancellationToken.None);

        // Assert
        Assert.IsType<OkObjectResult>(result);
    }

    [Fact]
    public async Task GetById_ReturnsNotFound_WhenMissing()
    {
        // Arrange
        _mediatorMock
            .Setup(m => m.Send(It.IsAny<GetClienteByIdQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((ClienteResponse?)null);
        var controller = new ClienteSystemController(_mediatorMock.Object);

        // Act
        var result = await controller.GetById(99, CancellationToken.None);

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }
}
