using Api.Contracts.Validation;
using CpfCnpjLibrary;
using Domain.Entities;
using Domain.ValueObjects;

namespace Api.Controllers.Staff.Cliente.CreateCliente;

public class CreateClienteRequestValidator : IValidator<CreateClienteRequest>
{
    public ValidationResult Validate(CreateClienteRequest request)
    {
        var result = new ValidationResult();

        if (string.IsNullOrWhiteSpace(request.Nome))
        {
            result.Errors.Add("Nome não pode estar vazio.");
        }

        if (string.IsNullOrWhiteSpace(request.Email))
        {
            result.Errors.Add("Email não pode estar vazio.");
        }

        if (!Enum.IsDefined(typeof(TipoDocumento), request.TipoDocumento))
        {
            result.Errors.Add("TipoDocumento é inválido.");
        }

        if (string.IsNullOrWhiteSpace(request.Documento))
        {
            result.Errors.Add("Documento não pode estar vazio.");
        }
        else
        {
            try
            {
                var tipoDocumento = request.TipoDocumento;
                var documentoClean = Documento.Criar(request.Documento).Valor;

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

        return result;
    }
}
