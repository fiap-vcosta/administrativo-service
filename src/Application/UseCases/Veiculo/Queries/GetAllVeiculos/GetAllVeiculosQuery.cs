using Application.UseCases.Veiculo.Responses;
using MediatR;

namespace Application.UseCases.Veiculo.Queries.GetAllVeiculos;

public class GetAllVeiculosQuery : IRequest<IEnumerable<VeiculoResponse>>;
