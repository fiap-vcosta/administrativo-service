using Application.UseCases.Cliente.Responses;
using MediatR;

namespace Application.UseCases.Cliente.Queries.GetAllClientes;

public class GetAllClientesQuery : IRequest<IEnumerable<ClienteResponse>>;
