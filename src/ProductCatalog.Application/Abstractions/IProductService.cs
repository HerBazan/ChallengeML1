using ProductCatalog.Application.DTOs;

namespace ProductCatalog.Application.Abstractions;

public interface IProductService
{
    ProductDto MapToDto(Domain.Entities.Product product);
}
