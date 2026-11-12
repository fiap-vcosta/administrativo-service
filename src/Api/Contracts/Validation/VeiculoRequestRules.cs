using System.Text.RegularExpressions;

namespace Api.Contracts.Validation;

public static partial class VeiculoRequestRules
{
    private static readonly Regex PlacaRegex = PlacaGeneratedRegex();

    public static void Validate(
        ValidationResult result,
        string? placa,
        int idCliente,
        string? modelo,
        string? marca)
    {
        if (string.IsNullOrWhiteSpace(placa))
        {
            result.Errors.Add("Placa não pode estar vazia.");
        }
        else if (!PlacaRegex.IsMatch(placa))
        {
            result.Errors.Add("Placa inválida.");
        }

        if (idCliente <= 0)
        {
            result.Errors.Add("IdCliente deve ser um cliente válido.");
        }

        if (string.IsNullOrWhiteSpace(modelo))
        {
            result.Errors.Add("Modelo não pode estar vazio.");
        }

        if (string.IsNullOrWhiteSpace(marca))
        {
            result.Errors.Add("Marca não pode estar vazia.");
        }
    }

    [GeneratedRegex(@"^[A-Za-z]{3}-?\d[A-Za-z0-9]\d{2}$")]
    private static partial Regex PlacaGeneratedRegex();
}
