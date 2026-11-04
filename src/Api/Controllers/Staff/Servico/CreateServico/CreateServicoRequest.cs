namespace Api.Controllers.Staff.Servico.CreateServico;

public class CreateServicoRequest
{
    public string Codigo { get; init; } = string.Empty;
    public string Nome { get; init; } = string.Empty;
    public required decimal PrecoPadrao { get; init; }
    public required bool Ativo { get; init; }
}
