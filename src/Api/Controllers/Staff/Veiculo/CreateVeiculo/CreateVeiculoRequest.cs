namespace Api.Controllers.Staff.Veiculo.CreateVeiculo;

public class CreateVeiculoRequest
{
    public string Placa { get; init; } = string.Empty;
    public int IdCliente { get; init; }
    public string Modelo { get; init; } = string.Empty;
    public string Marca { get; init; } = string.Empty;
}
