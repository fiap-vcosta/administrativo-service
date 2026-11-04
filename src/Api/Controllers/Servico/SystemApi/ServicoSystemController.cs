using Application.UseCases.Servico.Queries.GetServicoById;
using BuildingBlocks.Http.Auth;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.Servico.SystemApi;

[ApiController]
[AllowAnonymous]
[ServiceKeyAuthorize]
[Route("system/administrativo/servicos")]
public class ServicoSystemController(IMediator mediator) : ControllerBase
{
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id, CancellationToken cancellationToken)
    {
        var response = await mediator.Send(new GetServicoByIdQuery { Id = id }, cancellationToken);
        if (response is null)
        {
            return NotFound();
        }

        return Ok(new
        {
            id = response.Id,
            codigo = response.Codigo,
            nome = response.Nome,
            precoPadrao = response.PrecoPadrao
        });
    }
}
