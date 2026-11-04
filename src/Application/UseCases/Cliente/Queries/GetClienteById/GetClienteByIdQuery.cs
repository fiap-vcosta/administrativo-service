using Application.UseCases.Cliente.Responses;
using MediatR;

namespace Application.UseCases.Cliente.Queries.GetClienteById;

public class GetClienteByIdQuery : IRequest<ClienteResponse?>
{
    public int Id { get; init; }
}
