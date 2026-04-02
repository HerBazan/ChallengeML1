namespace ProductCatalog.Application.DTOs;

public record ProductDto(
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
    IReadOnlyDictionary<string, string> Specifications,
    string? BatteryCapacity,
    string? CameraSpecifications,
    string? Memory,
    string? StorageCapacity,
    string? Brand,
    string? ModelVersion,
    string? OperatingSystem,
    DateTime CreatedAtUtc);
