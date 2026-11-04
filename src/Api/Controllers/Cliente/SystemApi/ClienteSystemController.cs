using Api.Contracts.Validation;
using Application.UseCases.Cliente.Queries.ConsultarClientePorDocumento;
using Application.UseCases.Cliente.Queries.GetClienteById;
using BuildingBlocks.Http.Auth;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.Cliente.SystemApi;

[ApiController]
[AllowAnonymous]
[ServiceKeyAuthorize]
[Route("system/administrativo/clientes")]
public class ClienteSystemController(IMediator mediator) : ControllerBase
{
    [HttpGet("por-documento/{documento}")]
    public async Task<IActionResult> PorDocumento(string documento, CancellationToken cancellationToken)
    {
        if (!DocumentoValidator.TryNormalizeValidCpfOrCnpj(documento, out var normalized, out var errors))
        {
            return BadRequest(new { Errors = errors });
        }

        var existe = await mediator.Send(
            new ConsultarClientePorDocumentoQuery { Documento = normalized },
            cancellationToken);

        return existe ? Ok() : NotFound();
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id, CancellationToken cancellationToken)
    {
        var response = await mediator.Send(new GetClienteByIdQuery { Id = id }, cancellationToken);
        if (response is null)
        {
            return NotFound();
        }

        return Ok(new
        {
            id = response.Id,
            nome = response.Nome,
            email = response.Email,
            tipoDocumento = response.TipoDocumento.ToString(),
            documento = response.Documento
        });
    }
}
