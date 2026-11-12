using Api.Contracts.Validation;

namespace Api.Controllers.Staff.Cliente.CreateCliente;

public class CreateClienteRequestValidator : IValidator<CreateClienteRequest>
{
    public ValidationResult Validate(CreateClienteRequest request)
    {
        var result = new ValidationResult();
        ClienteRequestRules.Validate(
            result,
            request.Nome,
            request.Email,
            request.TipoDocumento,
            request.Documento);
        return result;
    }
}
