using Application.Abstractions.Gateways;
using Domain;
using MediatR;

namespace Application.UseCases.Cliente.Queries.ConsultarClientePorDocumento;

public class ConsultarClientePorDocumentoQueryHandler(IClienteGateway clienteGateway)
    : IRequestHandler<ConsultarClientePorDocumentoQuery, bool>
{
    public async Task<bool> Handle(
        ConsultarClientePorDocumentoQuery request,
        CancellationToken cancellationToken)
    {
        var documento = DocumentoNormalizer.Normalize(request.Documento);
        var cliente = await clienteGateway.GetByDocumentoAsync(documento);
        return cliente is not null;
    }
}
