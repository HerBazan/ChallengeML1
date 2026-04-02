using ProductCatalog.Application.Abstractions;
using ProductCatalog.Application.DTOs;

namespace ProductCatalog.Application.Features.Products.Queries;

public record GetProductByIdQuery(Guid Id) : IQuery<ProductDto?>;

public class GetProductByIdQueryHandler(IProductRepository repository, IProductService productService)
    : IQueryHandler<GetProductByIdQuery, ProductDto?>
{
    public async Task<ProductDto?> Handle(GetProductByIdQuery query, CancellationToken cancellationToken = default)
    {
        var product = await repository.GetByIdAsync(query.Id, cancellationToken);
        return product is null ? null : productService.MapToDto(product);
    }
}
