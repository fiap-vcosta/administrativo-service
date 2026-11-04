using Application.UseCases.Veiculo.Responses;
using MediatR;

namespace Application.UseCases.Veiculo.Queries.GetVeiculoById;

public class GetVeiculoByIdQuery : IRequest<VeiculoResponse?>
{
    public int Id { get; init; }
}
