using ProductCatalog.Domain.Entities;

namespace ProductCatalog.Infrastructure.Data;

public static class ProductSeeder
{
    public static async Task SeedAsync(AppDbContext context, CancellationToken cancellationToken = default)
    {
        if (context.Products.Any())
        {
            return;
        }

        var now = DateTime.UtcNow;
        var products = new List<Product>
        {
            new()
            {
                Id = Guid.NewGuid(),
                Name = "Phone X Pro",
                Description = "Smartphone de alta gama para comparación.",
                Price = 999.99m,
                Stock = 14,
                ImageUrl = "https://example.com/images/phone-x-pro.png",
                Rating = 4.7m,
                Size = "6.7 in",
                Weight = 0.21m,
                Color = "Graphite",
                Specifications = new Dictionary<string, string>
                {
                    ["screen"] = "AMOLED 120Hz",
                    ["cpu"] = "Octa-core",
                    ["connectivity"] = "5G"
                },
                BatteryCapacity = "5000 mAh",
                CameraSpecifications = "50MP + 12MP + 10MP",
                Memory = "12 GB",
                StorageCapacity = "256 GB",
                Brand = "Contoso",
                ModelVersion = "X Pro",
                OperatingSystem = "Android 15",
                CreatedAtUtc = now
            },
            new()
            {
                Id = Guid.NewGuid(),
                Name = "Phone Mini 5",
                Description = "Smartphone compacto para comparación.",
                Price = 699.50m,
                Stock = 20,
                ImageUrl = "https://example.com/images/phone-mini-5.png",
                Rating = 4.4m,
                Size = "6.1 in",
                Weight = 0.17m,
                Color = "Blue",
                Specifications = new Dictionary<string, string>
                {
                    ["screen"] = "OLED 90Hz",
                    ["cpu"] = "Hexa-core",
                    ["connectivity"] = "5G"
                },
                BatteryCapacity = "4200 mAh",
                CameraSpecifications = "48MP + 12MP",
                Memory = "8 GB",
                StorageCapacity = "128 GB",
                Brand = "Fabrikam",
                ModelVersion = "Mini 5",
                OperatingSystem = "Android 15",
                CreatedAtUtc = now
            },
            new()
            {
                Id = Guid.NewGuid(),
                Name = "Wireless Earbuds Air",
                Description = "Auriculares inalámbricos con cancelación de ruido.",
                Price = 179.90m,
                Stock = 55,
                ImageUrl = "https://example.com/images/earbuds-air.png",
                Rating = 4.3m,
                Size = "One size",
                Weight = 0.05m,
                Color = "White",
                Specifications = new Dictionary<string, string>
                {
                    ["batteryLife"] = "30h",
                    ["noiseCancellation"] = "Hybrid ANC",
                    ["bluetooth"] = "5.3"
                },
                Brand = "Tailspin",
                ModelVersion = "Air",
                CreatedAtUtc = now
            }
        };

        await context.Products.AddRangeAsync(products, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
    }
}
