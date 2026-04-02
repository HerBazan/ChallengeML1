namespace ProductCatalog.Domain.Entities;

public class Product
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public int Stock { get; set; }
    public string ImageUrl { get; set; } = string.Empty;
    public decimal Rating { get; set; }
    public string Size { get; set; } = string.Empty;
    public decimal Weight { get; set; }
    public string Color { get; set; } = string.Empty;
    public Dictionary<string, string> Specifications { get; set; } = new();
    public string? BatteryCapacity { get; set; }
    public string? CameraSpecifications { get; set; }
    public string? Memory { get; set; }
    public string? StorageCapacity { get; set; }
    public string? Brand { get; set; }
    public string? ModelVersion { get; set; }
    public string? OperatingSystem { get; set; }
    public DateTime CreatedAtUtc { get; set; }
}
