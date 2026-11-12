using Api.Contracts.Validation;

namespace Api.Controllers.Staff.Servico.UpdateServico;

public class UpdateServicoRequestValidator : IValidator<UpdateServicoRequest>
{
    public ValidationResult Validate(UpdateServicoRequest request)
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
