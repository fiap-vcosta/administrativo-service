using Application.UseCases.Servico.Responses;
using MediatR;

namespace Application.UseCases.Servico.Queries.GetServicoById;

public class GetServicoByIdQuery : IRequest<ServicoResponse?>
{
    public int Id { get; init; }
}
