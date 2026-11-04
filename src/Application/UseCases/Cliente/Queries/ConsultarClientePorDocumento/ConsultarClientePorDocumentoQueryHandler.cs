using Application.Abstractions.Gateways;
using Domain.ValueObjects;
using MediatR;

namespace Application.UseCases.Cliente.Queries.ConsultarClientePorDocumento;

public class ConsultarClientePorDocumentoQueryHandler(IClienteGateway clienteGateway)
    : IRequestHandler<ConsultarClientePorDocumentoQuery, bool>
{
    public async Task<bool> Handle(
        ConsultarClientePorDocumentoQuery request,
        CancellationToken cancellationToken)
    {
        var documento = Documento.Criar(request.Documento);
        var cliente = await clienteGateway.GetByDocumentoAsync(documento);
        return cliente is not null;
    }
}
