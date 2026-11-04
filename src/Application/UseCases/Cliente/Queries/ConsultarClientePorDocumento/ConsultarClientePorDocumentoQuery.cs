using MediatR;

namespace Application.UseCases.Cliente.Queries.ConsultarClientePorDocumento;

public class ConsultarClientePorDocumentoQuery : IRequest<bool>
{
    public required string Documento { get; init; }
}
