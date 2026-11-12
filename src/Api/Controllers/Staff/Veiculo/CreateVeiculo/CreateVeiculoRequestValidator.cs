using Api.Contracts.Validation;

namespace Api.Controllers.Staff.Veiculo.CreateVeiculo;

public class CreateVeiculoRequestValidator : IValidator<CreateVeiculoRequest>
{
    public ValidationResult Validate(CreateVeiculoRequest request)
    {
        var result = new ValidationResult();
        VeiculoRequestRules.Validate(
            result,
            request.Placa,
            request.IdCliente,
            request.Modelo,
            request.Marca);
        return result;
    }
}
