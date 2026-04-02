using ProductCatalog.Application.Abstractions;
using ProductCatalog.Application.DTOs;

namespace ProductCatalog.Application.Features.Products.Queries;

public record CompareProductsQuery(IReadOnlyList<Guid> Ids, IReadOnlyList<string>? Fields) : IQuery<CompareProductsResultDto>;

public class CompareProductsQueryHandler(IProductRepository repository) : IQueryHandler<CompareProductsQuery, CompareProductsResultDto>
{
    private static readonly string[] DefaultFields =
    [
        "description",
        "imageUrl",
        "price",
        "rating",
        "size",
        "weight",
        "color",
        "specifications"
    ];

    private static readonly HashSet<string> AllowedFields = new(
        [
            "description",
            "imageUrl",
            "price",
            "rating",
            "size",
            "weight",
            "color",
            "specifications",
            "batteryCapacity",
            "cameraSpecifications",
            "memory",
            "storageCapacity",
            "brand",
            "modelVersion",
            "operatingSystem"
        ],
        StringComparer.OrdinalIgnoreCase);

    public async Task<CompareProductsResultDto> Handle(CompareProductsQuery query, CancellationToken cancellationToken = default)
    {
        if (query.Ids.Count < 2)
        {
            throw new InvalidOperationException("Debes enviar al menos 2 productos para comparar.");
        }

        var fields = query.Fields is { Count: > 0 }
            ? query.Fields.Select(f => f.Trim()).Where(f => !string.IsNullOrWhiteSpace(f)).Distinct(StringComparer.OrdinalIgnoreCase).ToList()
            : DefaultFields.ToList();

        var invalidFields = fields.Where(f => !AllowedFields.Contains(f)).ToList();
        if (invalidFields.Count > 0)
        {
            throw new InvalidOperationException($"Campos no permitidos para comparar: {string.Join(", ", invalidFields)}.");
        }

        var requestedIds = query.Ids.Distinct().ToList();
        var products = await repository.GetAllAsync(cancellationToken);
        var productsById = products.Where(p => requestedIds.Contains(p.Id)).ToDictionary(x => x.Id);

        var missingIds = requestedIds.Where(id => !productsById.ContainsKey(id)).ToList();
        if (missingIds.Count > 0)
        {
            throw new KeyNotFoundException($"No se encontraron productos para los IDs: {string.Join(", ", missingIds)}.");
        }

        var items = requestedIds
            .Select(id => productsById[id])
            .Select(product => new ProductComparisonItemDto(
                product.Id,
                product.Name,
                BuildAttributes(product, fields)))
            .ToList();

        return new CompareProductsResultDto(fields, items);
    }

    private static IReadOnlyDictionary<string, object?> BuildAttributes(Domain.Entities.Product product, IReadOnlyList<string> fields)
    {
        var attributes = new Dictionary<string, object?>(StringComparer.OrdinalIgnoreCase);

        foreach (var field in fields)
        {
            attributes[field] = field switch
            {
                "description" => product.Description,
                "imageUrl" => product.ImageUrl,
                "price" => product.Price,
                "rating" => product.Rating,
                "size" => product.Size,
                "weight" => product.Weight,
                "color" => product.Color,
                "specifications" => product.Specifications,
                "batteryCapacity" => product.BatteryCapacity,
                "cameraSpecifications" => product.CameraSpecifications,
                "memory" => product.Memory,
                "storageCapacity" => product.StorageCapacity,
                "brand" => product.Brand,
                "modelVersion" => product.ModelVersion,
                "operatingSystem" => product.OperatingSystem,
                _ => null
            };
        }

        return attributes;
    }
}
