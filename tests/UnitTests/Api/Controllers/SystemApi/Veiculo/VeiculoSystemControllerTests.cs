using Api.Controllers.SystemApi.Veiculo;
using Application.UseCases.Veiculo.Queries.GetVeiculoById;
using Application.UseCases.Veiculo.Responses;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace UnitTests.Api.Controllers.SystemApi.Veiculo;

public class VeiculoSystemControllerTests
{
    private readonly Mock<IMediator> _mediator = new();
    private readonly VeiculoSystemController _controller;

    public VeiculoSystemControllerTests()
    {
        _controller = new VeiculoSystemController(_mediator.Object);
    }

    [Fact]
    public async Task GetById_ReturnsOk_WhenFound()
    {
        // Arrange
        _mediator.Setup(m => m.Send(It.IsAny<GetVeiculoByIdQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new VeiculoResponse
            {
                Id = 1,
                IdCliente = 2,
                Placa = "ABC1D23",
                Marca = "VW",
                Modelo = "Gol"
            });

        // Act
        var result = await _controller.GetById(1, CancellationToken.None);

        // Assert
        Assert.IsType<OkObjectResult>(result);
    }

    [Fact]
    public async Task GetById_ReturnsNotFound_WhenMissing()
    {
        // Arrange
        _mediator.Setup(m => m.Send(It.IsAny<GetVeiculoByIdQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((VeiculoResponse?)null);

        // Act
        var result = await _controller.GetById(99, CancellationToken.None);

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }
}
