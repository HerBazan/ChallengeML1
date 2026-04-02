using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using ProductCatalog.Domain.Entities;
using System.Text.Json;

namespace ProductCatalog.Infrastructure.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Product> Products => Set<Product>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(x => x.Id);
            entity.Property(x => x.Name).HasMaxLength(120).IsRequired();
            entity.Property(x => x.Description).HasMaxLength(600);
            entity.Property(x => x.Price).HasColumnType("decimal(18,2)");
            entity.Property(x => x.Rating).HasColumnType("decimal(3,2)");
            entity.Property(x => x.ImageUrl).HasMaxLength(600);
            entity.Property(x => x.Size).HasMaxLength(100);
            entity.Property(x => x.Weight).HasColumnType("decimal(10,2)");
            entity.Property(x => x.Color).HasMaxLength(80);
            entity.Property(x => x.BatteryCapacity).HasMaxLength(80);
            entity.Property(x => x.CameraSpecifications).HasMaxLength(200);
            entity.Property(x => x.Memory).HasMaxLength(80);
            entity.Property(x => x.StorageCapacity).HasMaxLength(80);
            entity.Property(x => x.Brand).HasMaxLength(120);
            entity.Property(x => x.ModelVersion).HasMaxLength(120);
            entity.Property(x => x.OperatingSystem).HasMaxLength(120);
            entity.Property(x => x.Specifications)
                .HasConversion(
                    v => JsonSerializer.Serialize(v, (JsonSerializerOptions?)null),
                    v => string.IsNullOrWhiteSpace(v)
                        ? new Dictionary<string, string>()
                        : JsonSerializer.Deserialize<Dictionary<string, string>>(v, (JsonSerializerOptions?)null) ?? new Dictionary<string, string>())
                .Metadata.SetValueComparer(new ValueComparer<Dictionary<string, string>>(
                    (c1, c2) => JsonSerializer.Serialize(c1, (JsonSerializerOptions?)null) == JsonSerializer.Serialize(c2, (JsonSerializerOptions?)null),
                    c => JsonSerializer.Serialize(c, (JsonSerializerOptions?)null).GetHashCode(),
                    c => JsonSerializer.Deserialize<Dictionary<string, string>>(
                        JsonSerializer.Serialize(c, (JsonSerializerOptions?)null),
                        (JsonSerializerOptions?)null) ?? new Dictionary<string, string>()));
            entity.Property(x => x.CreatedAtUtc).IsRequired();
            entity.HasIndex(x => x.Name).IsUnique();
        });
    }
}
