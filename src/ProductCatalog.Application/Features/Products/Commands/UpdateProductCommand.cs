using ProductCatalog.Application.Abstractions;
using ProductCatalog.Application.DTOs;

namespace ProductCatalog.Application.Features.Products.Commands;

public record UpdateProductCommand(
    Guid Id,
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

public class UpdateProductCommandHandler(IProductRepository repository, IProductService productService)
    : ICommandHandler<UpdateProductCommand, ProductDto>
{
    public async Task<ProductDto> Handle(UpdateProductCommand command, CancellationToken cancellationToken = default)
    {
        var product = await repository.GetByIdAsync(command.Id, cancellationToken)
            ?? throw new KeyNotFoundException($"No se encontró el producto con Id '{command.Id}'.");

        product.Name = command.Name;
        product.Description = command.Description;
        product.Price = command.Price;
        product.Stock = command.Stock;
        product.ImageUrl = command.ImageUrl;
        product.Rating = command.Rating;
        product.Size = command.Size;
        product.Weight = command.Weight;
        product.Color = command.Color;
        product.Specifications = command.Specifications ?? new Dictionary<string, string>();
        product.BatteryCapacity = command.BatteryCapacity;
        product.CameraSpecifications = command.CameraSpecifications;
        product.Memory = command.Memory;
        product.StorageCapacity = command.StorageCapacity;
        product.Brand = command.Brand;
        product.ModelVersion = command.ModelVersion;
        product.OperatingSystem = command.OperatingSystem;

        await repository.UpdateAsync(product, cancellationToken);

        return productService.MapToDto(product);
    }
}
