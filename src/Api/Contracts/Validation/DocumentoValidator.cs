using CpfCnpjLibrary;
using Domain.Exceptions;
using Domain.ValueObjects;

namespace Api.Contracts.Validation;

public static class DocumentoValidator
{
    public static bool TryNormalizeValidCpfOrCnpj(
        string? documento,
        out string normalized,
        out IReadOnlyList<string> errors)
    {
        var errorList = new List<string>();
        normalized = string.Empty;

        if (string.IsNullOrWhiteSpace(documento))
        {
            errorList.Add("Documento é obrigatório.");
            errors = errorList;
            return false;
        }

        try
        {
            normalized = Documento.Criar(documento).Valor;
        }
        catch (BusinessRuleException ex)
        {
            errorList.Add(ex.Message);
            errors = errorList;
            return false;
        }

        if (!Cpf.Validar(normalized) && !Cnpj.Validar(normalized))
        {
            errorList.Add("Documento inválido.");
            errors = errorList;
            return false;
        }

        errors = errorList;
        return true;
    }
}
