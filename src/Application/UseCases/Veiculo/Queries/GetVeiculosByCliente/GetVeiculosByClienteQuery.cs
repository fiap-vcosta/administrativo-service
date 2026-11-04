using Application.UseCases.Veiculo.Responses;
using MediatR;

namespace Application.UseCases.Veiculo.Queries.GetVeiculosByCliente;

public class GetVeiculosByClienteQuery : IRequest<IEnumerable<VeiculoResponse>>
{
    public int IdCliente { get; init; }
}
