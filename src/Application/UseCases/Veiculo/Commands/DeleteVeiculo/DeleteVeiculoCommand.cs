using MediatR;

namespace Application.UseCases.Veiculo.Commands.DeleteVeiculo;

public class DeleteVeiculoCommand : IRequest<Unit>
{
    public int Id { get; init; }
}
