using Domain.ValueObjects;

namespace Domain.Entities;

public enum TipoDocumento
{
    Cpf,
    Cnpj
}

public class ClienteAggregateRoot
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public TipoDocumento TipoDocumento { get; set; }
    public Documento Documento { get; set; } = null!;
}
