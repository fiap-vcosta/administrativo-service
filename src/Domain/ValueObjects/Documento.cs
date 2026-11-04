using Domain.Exceptions;

namespace Domain.ValueObjects;

public sealed record Documento
{
    public string Valor { get; }

    private Documento(string valor) => Valor = valor;

    public static Documento Criar(string documento)
    {
        if (string.IsNullOrWhiteSpace(documento))
        {
            throw new BusinessRuleException("Documento é obrigatório.");
        }

        var normalizado = Normalizar(documento);
        if (normalizado.Length == 0)
        {
            throw new BusinessRuleException("Documento é obrigatório.");
        }

        return new Documento(normalizado);
    }

    private static string Normalizar(string documento) =>
        documento.Replace(".", "").Replace("-", "").Replace("/", "").Trim();

    public override string ToString() => Valor;
}
