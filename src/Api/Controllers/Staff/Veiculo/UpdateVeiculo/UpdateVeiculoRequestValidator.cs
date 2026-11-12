using Api.Contracts.Validation;

namespace Api.Controllers.Staff.Veiculo.UpdateVeiculo;

public class UpdateVeiculoRequestValidator : IValidator<UpdateVeiculoRequest>
{
    public ValidationResult Validate(UpdateVeiculoRequest request)
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
