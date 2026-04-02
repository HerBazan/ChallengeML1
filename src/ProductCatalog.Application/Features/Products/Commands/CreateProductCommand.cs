using ProductCatalog.Application.Abstractions;
using ProductCatalog.Application.DTOs;
using ProductCatalog.Domain.Entities;

namespace ProductCatalog.Application.Features.Products.Commands;

public record CreateProductCommand(
    string Name,
    string Description,
    decimal Price,
    int Stock,
    string ImageUrl,
    decimal Rating,
    string Size,
    decimal Weight,
    string Color,
    Dictionary<string, string>? Specifications,
    string? BatteryCapacity,
    string? CameraSpecifications,
    string? Memory,
    string? StorageCapacity,
    string? Brand,
    string? ModelVersion,
    string? OperatingSystem) : ICommand<ProductDto>;

public class CreateProductCommandHandler(IProductRepository repository, IProductService productService)
    : ICommandHandler<CreateProductCommand, ProductDto>
{
    public async Task<ProductDto> Handle(CreateProductCommand command, CancellationToken cancellationToken = default)
    {
        if (await repository.ExistsByNameAsync(command.Name, cancellationToken))
        {
            throw new InvalidOperationException($"Ya existe un producto con nombre '{command.Name}'.");
        }

        var product = new Product
        {
            Id = Guid.NewGuid(),
            Name = command.Name,
            Description = command.Description,
            Price = command.Price,
            Stock = command.Stock,
            ImageUrl = command.ImageUrl,
            Rating = command.Rating,
            Size = command.Size,
            Weight = command.Weight,
            Color = command.Color,
            Specifications = command.Specifications ?? new Dictionary<string, string>(),
            BatteryCapacity = command.BatteryCapacity,
            CameraSpecifications = command.CameraSpecifications,
            Memory = command.Memory,
            StorageCapacity = command.StorageCapacity,
            Brand = command.Brand,
            ModelVersion = command.ModelVersion,
            OperatingSystem = command.OperatingSystem,
            CreatedAtUtc = DateTime.UtcNow
        };

        await repository.AddAsync(product, cancellationToken);

        return productService.MapToDto(product);
    }
}
