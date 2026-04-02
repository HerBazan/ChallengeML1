using ProductCatalog.Application.Abstractions;

namespace ProductCatalog.Application.Features.Products.Commands;

public record DeleteProductCommand(Guid Id) : ICommand<bool>;

public class DeleteProductCommandHandler(IProductRepository repository)
    : ICommandHandler<DeleteProductCommand, bool>
{
    public async Task<bool> Handle(DeleteProductCommand command, CancellationToken cancellationToken = default)
    {
        var product = await repository.GetByIdAsync(command.Id, cancellationToken)
            ?? throw new KeyNotFoundException($"No se encontró el producto con Id '{command.Id}'.");

        await repository.DeleteAsync(product, cancellationToken);

        return true;
    }
}
