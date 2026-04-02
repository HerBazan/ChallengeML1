namespace ProductCatalog.Application.DTOs;

public record ProductComparisonItemDto(
    Guid Id,
    string Name,
    IReadOnlyDictionary<string, object?> Attributes);

public record CompareProductsResultDto(
    IReadOnlyList<string> Fields,
    IReadOnlyList<ProductComparisonItemDto> Items);
