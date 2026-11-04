using Domain.Entities;
using Application.UseCases.Veiculo.Responses;

namespace Application.UseCases.Cliente.Responses;

public class ClienteResponse
{
    public int Id { get; init; }
    public string Nome { get; init; } = string.Empty;
    public string Email { get; init; } = string.Empty;
    public TipoDocumento TipoDocumento { get; init; }
    public string Documento { get; init; } = string.Empty;
    public IReadOnlyList<VeiculoResponse> Veiculos { get; init; } = [];
}
