using System.Text.RegularExpressions;

namespace Api.Contracts.Validation;

public static partial class ServicoRequestRules
{
    private static readonly Regex CodigoRegex = CodigoGeneratedRegex();

    public static void Validate(
        ValidationResult result,
        string? codigo,
        string? nome,
        decimal precoPadrao)
    {
        if (string.IsNullOrWhiteSpace(codigo))
        {
            result.Errors.Add("Código não pode estar vazio.");
        }
        else if (!CodigoRegex.IsMatch(codigo))
        {
            result.Errors.Add("Código inválido. Formato esperado: AAA-123");
        }

        if (string.IsNullOrWhiteSpace(nome))
        {
            result.Errors.Add("Nome não pode estar vazio.");
        }

        if (precoPadrao <= 0)
        {
            result.Errors.Add("Preço padrão deve ser maior que zero.");
        }
    }

    [GeneratedRegex(@"^[A-Z]{3}-\d{3}$")]
    private static partial Regex CodigoGeneratedRegex();
}
