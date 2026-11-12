using Api.Contracts.Validation;

namespace Api.Controllers.Staff.Cliente.UpdateCliente;

public class UpdateClienteRequestValidator : IValidator<UpdateClienteRequest>
{
    public ValidationResult Validate(UpdateClienteRequest request)
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
