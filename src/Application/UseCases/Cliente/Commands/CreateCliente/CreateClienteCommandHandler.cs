using Application.Abstractions.Gateways;
using Application.UseCases.Cliente.Responses;
using Domain;
using Domain.Entities;
using Domain.Exceptions;
using MediatR;

namespace Application.UseCases.Cliente.Commands.CreateCliente;

public class CreateClienteCommandHandler(IClienteGateway clienteGateway)
    : IRequestHandler<CreateClienteCommand, ClienteResponse>
{
    public async Task<ClienteResponse> Handle(CreateClienteCommand request, CancellationToken cancellationToken)
    {
        var documento = DocumentoNormalizer.Normalize(request.Documento);
        var existingCliente = await clienteGateway.GetByDocumentoAsync(documento);

        if (existingCliente != null)
        {
            throw new BusinessRuleException("Já existe um cliente com este documento.");
        }

        var cliente = new ClienteAggregateRoot
        {
            Nome = request.Nome,
            Email = request.Email,
            TipoDocumento = request.TipoDocumento,
            Documento = documento
        };

        await clienteGateway.CreateAsync(cliente);

        return new ClienteResponse
        {
            Id = cliente.Id,
            Nome = cliente.Nome,
            Email = cliente.Email,
            TipoDocumento = cliente.TipoDocumento,
            Documento = cliente.Documento
        };
    }
}
