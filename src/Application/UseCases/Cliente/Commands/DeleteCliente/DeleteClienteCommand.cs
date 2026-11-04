using MediatR;

namespace Application.UseCases.Cliente.Commands.DeleteCliente;

public class DeleteClienteCommand : IRequest<Unit>
{
    public int Id { get; init; }
}
