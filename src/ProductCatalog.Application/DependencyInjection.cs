using Microsoft.Extensions.DependencyInjection;
using ProductCatalog.Application.Abstractions;
using ProductCatalog.Application.Features.Products.Commands;
using ProductCatalog.Application.Features.Products.Queries;

namespace ProductCatalog.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<ICommandDispatcher, CommandDispatcher>();
        services.AddScoped<IQueryDispatcher, QueryDispatcher>();

        services.AddScoped<ICommandHandler<CreateProductCommand, DTOs.ProductDto>, CreateProductCommandHandler>();
        services.AddScoped<ICommandHandler<UpdateProductCommand, DTOs.ProductDto>, UpdateProductCommandHandler>();
        services.AddScoped<ICommandHandler<DeleteProductCommand, bool>, DeleteProductCommandHandler>();

        services.AddScoped<IQueryHandler<GetProductsQuery, IReadOnlyList<DTOs.ProductDto>>, GetProductsQueryHandler>();
        services.AddScoped<IQueryHandler<GetProductByIdQuery, DTOs.ProductDto?>, GetProductByIdQueryHandler>();
        services.AddScoped<IQueryHandler<CompareProductsQuery, DTOs.CompareProductsResultDto>, CompareProductsQueryHandler>();

        return services;
    }
}
