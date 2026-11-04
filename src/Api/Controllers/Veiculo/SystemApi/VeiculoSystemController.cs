using Application.UseCases.Veiculo.Queries.GetVeiculoById;
using BuildingBlocks.Http.Auth;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.Veiculo.SystemApi;

[ApiController]
[AllowAnonymous]
[ServiceKeyAuthorize]
[Route("system/administrativo/veiculos")]
public class VeiculoSystemController(IMediator mediator) : ControllerBase
{
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id, CancellationToken cancellationToken)
    {
        var response = await mediator.Send(new GetVeiculoByIdQuery { Id = id }, cancellationToken);
        if (response is null)
        {
            return NotFound();
        }

        return Ok(new
        {
            id = response.Id,
            idCliente = response.IdCliente,
            placa = response.Placa,
            marca = response.Marca,
            modelo = response.Modelo
        });
    }
}
