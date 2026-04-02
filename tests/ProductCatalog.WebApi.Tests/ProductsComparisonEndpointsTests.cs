using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace ProductCatalog.WebApi.Tests;

public class ProductsComparisonEndpointsTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public ProductsComparisonEndpointsTests(WebApplicationFactory<Program> factory)
    {
        _client = factory.WithWebHostBuilder(_ => { }).CreateClient(new WebApplicationFactoryClientOptions
        {
            BaseAddress = new Uri("https://localhost")
        });
    }

    [Fact]
    public async Task GetProducts_ReturnsSeededProducts()
    {
        var products = await _client.GetFromJsonAsync<List<ProductResponse>>("/api/products");

        Assert.NotNull(products);
        Assert.True(products!.Count >= 3);
    }

    [Fact]
    public async Task CompareProducts_WithFields_ReturnsOnlyRequestedAttributes()
    {
        var products = await _client.GetFromJsonAsync<List<ProductResponse>>("/api/products");
        Assert.NotNull(products);
        Assert.True(products!.Count >= 2);

        var ids = string.Join(",", products.Take(2).Select(x => x.Id));
        var response = await _client.GetFromJsonAsync<CompareResponse>(
            $"/api/products/compare?ids={ids}&fields=price,rating,batteryCapacity");

        Assert.NotNull(response);
        Assert.Equal(2, response!.Items.Count);
        Assert.Equal(3, response.Fields.Count);
        Assert.All(response.Items, item =>
        {
            Assert.Equal(3, item.Attributes.Count);
            Assert.Contains("price", item.Attributes.Keys, StringComparer.OrdinalIgnoreCase);
            Assert.Contains("rating", item.Attributes.Keys, StringComparer.OrdinalIgnoreCase);
            Assert.Contains("batteryCapacity", item.Attributes.Keys, StringComparer.OrdinalIgnoreCase);
        });
    }

    [Fact]
    public async Task CompareProducts_WithInvalidField_ReturnsBadRequest()
    {
        var products = await _client.GetFromJsonAsync<List<ProductResponse>>("/api/products");
        Assert.NotNull(products);
        Assert.True(products!.Count >= 2);

        var ids = string.Join(",", products.Take(2).Select(x => x.Id));
        var response = await _client.GetAsync($"/api/products/compare?ids={ids}&fields=price,notValidField");

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task CompareProducts_WithMoreThanTenIds_ReturnsBadRequest()
    {
        var ids = string.Join(",", Enumerable.Range(0, 11).Select(_ => Guid.NewGuid()));
        var response = await _client.GetAsync($"/api/products/compare?ids={ids}");

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    private sealed record ProductResponse(Guid Id, string Name);

    private sealed record CompareResponse(List<string> Fields, List<CompareItem> Items);

    private sealed record CompareItem(Guid Id, string Name, Dictionary<string, object?> Attributes);
}
