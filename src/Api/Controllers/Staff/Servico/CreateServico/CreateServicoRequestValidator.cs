using Api.Contracts.Validation;

namespace Api.Controllers.Staff.Servico.CreateServico;

public class CreateServicoRequestValidator : IValidator<CreateServicoRequest>
{
    public ValidationResult Validate(CreateServicoRequest request)
    {
        var result = new ValidationResult();
        ServicoRequestRules.Validate(
            result,
            request.Codigo,
            request.Nome,
            request.PrecoPadrao);
        return result;
    }
}
