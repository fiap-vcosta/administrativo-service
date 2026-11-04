using Domain.Entities;
using Domain.ValueObjects;

namespace Application.Abstractions.Gateways;

public interface IClienteGateway
{
    Task<IEnumerable<ClienteAggregateRoot>> GetAllAsync();
    Task<ClienteAggregateRoot?> GetByIdAsync(int id);
    Task<ClienteAggregateRoot?> GetByDocumentoAsync(Documento documento);
    Task CreateAsync(ClienteAggregateRoot clienteAggregateRoot);
    Task UpdateAsync(ClienteAggregateRoot clienteAggregateRoot);
    Task DeleteAsync(int id);
}
