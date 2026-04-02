using ProductCatalog.Application.Abstractions;
using ProductCatalog.Application.DTOs;

namespace ProductCatalog.Application.Features.Products.Queries;

public record GetProductsQuery : IQuery<IReadOnlyList<ProductDto>>;

public class GetProductsQueryHandler(IProductRepository repository, IProductService productService)
    : IQueryHandler<GetProductsQuery, IReadOnlyList<ProductDto>>
{
    public async Task<IReadOnlyList<ProductDto>> Handle(GetProductsQuery query, CancellationToken cancellationToken = default)
    {
        var products = await repository.GetAllAsync(cancellationToken);
        return products.Select(productService.MapToDto).ToList();
    }
}
