using Api.Controllers.SystemApi.Servico;
using Application.UseCases.Servico.Queries.GetServicoById;
using Application.UseCases.Servico.Responses;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace UnitTests.Api.Controllers.SystemApi.Servico;

public class ServicoSystemControllerTests
{
    private readonly Mock<IMediator> _mediator = new();
    private readonly ServicoSystemController _controller;

    public ServicoSystemControllerTests()
    {
        _controller = new ServicoSystemController(_mediator.Object);
    }

    [Fact]
    public async Task GetById_ReturnsOk_WhenFound()
    {
        // Arrange
        _mediator.Setup(m => m.Send(It.IsAny<GetServicoByIdQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ServicoResponse
            {
                Id = 1,
                Codigo = "OLE-1",
                Nome = "Oleo",
                PrecoPadrao = 10m,
                Ativo = true
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
        _mediator.Setup(m => m.Send(It.IsAny<GetServicoByIdQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((ServicoResponse?)null);

        // Act
        var result = await _controller.GetById(99, CancellationToken.None);

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }
}
