using CpfCnpjLibrary;
using Domain.Entities;
using Domain.ValueObjects;

namespace Api.Contracts.Validation;

public static class ClienteRequestRules
{
    public static void Validate(
        ValidationResult result,
        string? nome,
        string? email,
        TipoDocumento tipoDocumento,
        string? documento)
    {
        if (string.IsNullOrWhiteSpace(nome))
        {
            result.Errors.Add("Nome não pode estar vazio.");
        }

        if (string.IsNullOrWhiteSpace(email))
        {
            result.Errors.Add("Email não pode estar vazio.");
        }

        if (!Enum.IsDefined(typeof(TipoDocumento), tipoDocumento))
        {
            result.Errors.Add("TipoDocumento é inválido.");
        }

        if (string.IsNullOrWhiteSpace(documento))
        {
            result.Errors.Add("Documento não pode estar vazio.");
            return;
        }

        try
        {
            var documentoClean = Documento.Criar(documento).Valor;

            switch (tipoDocumento)
            {
                case TipoDocumento.Cpf:
                {
                    if (!Cpf.Validar(documentoClean))
                    {
                        result.Errors.Add("CPF inválido.");
                    }

                    break;
                }
                case TipoDocumento.Cnpj:
                {
                    if (!Cnpj.Validar(documentoClean))
                    {
                        result.Errors.Add("CNPJ inválido.");
                    }

                    break;
                }
            }
        }
        catch (Domain.Exceptions.BusinessRuleException ex)
        {
            result.Errors.Add(ex.Message);
        }
    }
}
