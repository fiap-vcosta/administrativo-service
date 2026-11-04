using Domain.Entities;

namespace Api.Controllers.Staff.Cliente.UpdateCliente;

public class UpdateClienteRequest
{
    public string Nome { get; init; } = string.Empty;
    public string Email { get; init; } = string.Empty;
    public TipoDocumento TipoDocumento { get; init; }
    public string Documento { get; init; } = string.Empty;
}
