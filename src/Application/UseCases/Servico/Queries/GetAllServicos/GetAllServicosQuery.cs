using Application.UseCases.Servico.Responses;
using MediatR;

namespace Application.UseCases.Servico.Queries.GetAllServicos;

public class GetAllServicosQuery : IRequest<IEnumerable<ServicoResponse>>;
