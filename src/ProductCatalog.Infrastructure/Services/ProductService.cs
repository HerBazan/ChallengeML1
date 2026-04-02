using ProductCatalog.Application.Abstractions;
using ProductCatalog.Application.DTOs;
using ProductCatalog.Domain.Entities;

namespace ProductCatalog.Infrastructure.Services;

public class ProductService : IProductService
{
    public ProductDto MapToDto(Product product) =>
        new(
            product.Id,
            product.Name,
            product.Description,
            product.Price,
            product.Stock,
            product.ImageUrl,
            product.Rating,
            product.Size,
            product.Weight,
            product.Color,
            product.Specifications,
            product.BatteryCapacity,
            product.CameraSpecifications,
            product.Memory,
            product.StorageCapacity,
            product.Brand,
            product.ModelVersion,
            product.OperatingSystem,
            product.CreatedAtUtc);
}
