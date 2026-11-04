using Application.UseCases.Cliente.Responses;
using Domain.Entities;
using MediatR;

namespace Application.UseCases.Cliente.Commands.UpdateCliente;

public class UpdateClienteCommand : IRequest<ClienteResponse>
{
    public int Id { get; init; }
    public string Nome { get; init; } = string.Empty;
    public string Email { get; init; } = string.Empty;
    public TipoDocumento TipoDocumento { get; init; }
    public string Documento { get; init; } = string.Empty;
}
