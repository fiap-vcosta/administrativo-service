using Application.Abstractions.Gateways;
using Application.UseCases.Cliente.Responses;
using Domain.Exceptions;
using Domain.ValueObjects;
using MediatR;

namespace Application.UseCases.Cliente.Commands.UpdateCliente;

public class UpdateClienteCommandHandler(IClienteGateway clienteGateway)
    : IRequestHandler<UpdateClienteCommand, ClienteResponse>
{
    public async Task<ClienteResponse> Handle(UpdateClienteCommand request, CancellationToken cancellationToken)
    {
        var cliente = await clienteGateway.GetByIdAsync(request.Id);
        if (cliente == null)
        {
            throw new DomainNotFoundException($"Cliente com id {request.Id} não encontrado");
        }

        var documento = Documento.Criar(request.Documento);
        var existingCliente = await clienteGateway.GetByDocumentoAsync(documento);
        if (existingCliente != null && existingCliente.Id != cliente.Id)
        {
            throw new BusinessRuleException("Já existe um cliente com este documento.");
        }

        cliente.Nome = request.Nome;
        cliente.Email = request.Email;
        cliente.TipoDocumento = request.TipoDocumento;
        cliente.Documento = documento;

        await clienteGateway.UpdateAsync(cliente);

        return new ClienteResponse
        {
            Id = cliente.Id,
            Nome = cliente.Nome,
            Email = cliente.Email,
            TipoDocumento = cliente.TipoDocumento,
            Documento = cliente.Documento.Valor
        };
    }
}
