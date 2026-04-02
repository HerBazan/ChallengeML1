using ProductCatalog.Application.Abstractions;
using ProductCatalog.Application.Features.Products.Queries;
using ProductCatalog.Domain.Entities;
using Xunit;

namespace ProductCatalog.Application.Tests.Features.Products.Queries;

public class CompareProductsQueryHandlerTests
{
    [Fact]
    public async Task Handle_WhenFieldsAreProvided_ReturnsOnlyRequestedAttributes()
    {
        var products = BuildProducts();
        var repository = new InMemoryProductRepository(products);
        var handler = new CompareProductsQueryHandler(repository);

        var query = new CompareProductsQuery(
            [products[0].Id, products[1].Id],
            ["price", "rating"]);

        var result = await handler.Handle(query);

        Assert.Equal(2, result.Items.Count);
        Assert.Equal(2, result.Fields.Count);
        Assert.All(result.Items, item =>
        {
            Assert.Equal(2, item.Attributes.Count);
            Assert.True(item.Attributes.ContainsKey("price"));
            Assert.True(item.Attributes.ContainsKey("rating"));
        });
    }

    [Fact]
    public async Task Handle_WhenFieldIsInvalid_ThrowsInvalidOperationException()
    {
        var repository = new InMemoryProductRepository(BuildProducts());
        var handler = new CompareProductsQueryHandler(repository);
        var query = new CompareProductsQuery([Guid.NewGuid(), Guid.NewGuid()], ["invalidField"]);

        await Assert.ThrowsAsync<InvalidOperationException>(() => handler.Handle(query));
    }

    [Fact]
    public async Task Handle_WhenAnyProductIsMissing_ThrowsKeyNotFoundException()
    {
        var products = BuildProducts();
        var repository = new InMemoryProductRepository(products);
        var handler = new CompareProductsQueryHandler(repository);

        var query = new CompareProductsQuery([products[0].Id, Guid.NewGuid()], ["price"]);

        await Assert.ThrowsAsync<KeyNotFoundException>(() => handler.Handle(query));
    }

    private static List<Product> BuildProducts() =>
    [
        new()
        {
            Id = Guid.NewGuid(),
            Name = "Phone A",
            Description = "A",
            Price = 800m,
            Rating = 4.6m,
            Size = "6.5",
            Weight = 0.2m,
            Color = "Black",
            Specifications = new Dictionary<string, string> { ["cpu"] = "A1" }
        },
        new()
        {
            Id = Guid.NewGuid(),
            Name = "Phone B",
            Description = "B",
            Price = 650m,
            Rating = 4.2m,
            Size = "6.1",
            Weight = 0.18m,
            Color = "Blue",
            Specifications = new Dictionary<string, string> { ["cpu"] = "B1" }
        }
    ];

    private sealed class InMemoryProductRepository(List<Product> products) : IProductRepository
    {
        public Task<Product?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default) =>
            Task.FromResult(products.FirstOrDefault(x => x.Id == id));

        public Task<IReadOnlyList<Product>> GetAllAsync(CancellationToken cancellationToken = default) =>
            Task.FromResult<IReadOnlyList<Product>>(products);

        public Task AddAsync(Product product, CancellationToken cancellationToken = default) => throw new NotImplementedException();
        public Task UpdateAsync(Product product, CancellationToken cancellationToken = default) => throw new NotImplementedException();
        public Task DeleteAsync(Product product, CancellationToken cancellationToken = default) => throw new NotImplementedException();
        public Task<bool> ExistsByNameAsync(string name, CancellationToken cancellationToken = default) => throw new NotImplementedException();
    }
}
