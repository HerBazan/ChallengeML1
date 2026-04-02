using Microsoft.AspNetCore.Mvc;
using ProductCatalog.Application.Abstractions;
using ProductCatalog.Application.DTOs;
using ProductCatalog.Application.Features.Products.Commands;
using ProductCatalog.Application.Features.Products.Queries;

namespace ProductCatalog.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController(ICommandDispatcher commandDispatcher, IQueryDispatcher queryDispatcher) : ControllerBase
{
    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyList<ProductDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var result = await queryDispatcher.Dispatch<GetProductsQuery, IReadOnlyList<ProductDto>>(new GetProductsQuery(), cancellationToken);
        return Ok(result);
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(ProductDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
    {
        var result = await queryDispatcher.Dispatch<GetProductByIdQuery, ProductDto?>(new GetProductByIdQuery(id), cancellationToken);
        return result is null ? NotFound() : Ok(result);
    }

    [HttpGet("compare")]
    [ProducesResponseType(typeof(CompareProductsResultDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Compare([FromQuery] string ids, [FromQuery] string? fields, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(ids))
        {
            return BadRequest(new { message = "El parámetro 'ids' es obligatorio." });
        }

        var parsedIds = ids
            .Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
            .Select(raw => Guid.TryParse(raw, out var id) ? id : Guid.Empty)
            .ToList();

        if (parsedIds.Any(id => id == Guid.Empty))
        {
            return BadRequest(new { message = "Todos los valores de 'ids' deben ser GUIDs válidos." });
        }

        if (parsedIds.Count > 10)
        {
            return BadRequest(new { message = "El parámetro 'ids' admite un máximo de 10 productos para comparar." });
        }

        var parsedFields = string.IsNullOrWhiteSpace(fields)
            ? null
            : fields
                .Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
                .ToList();

        var query = new CompareProductsQuery(parsedIds, parsedFields);
        var result = await queryDispatcher.Dispatch<CompareProductsQuery, CompareProductsResultDto>(query, cancellationToken);
        return Ok(result);
    }

    [HttpPost]
    [ProducesResponseType(typeof(ProductDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] CreateProductCommand command, CancellationToken cancellationToken)
    {
        var result = await commandDispatcher.Dispatch<CreateProductCommand, ProductDto>(command, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    [HttpPut("{id:guid}")]
    [ProducesResponseType(typeof(ProductDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateProductRequest request, CancellationToken cancellationToken)
    {
        var command = new UpdateProductCommand(
            id,
            request.Name,
            request.Description,
            request.Price,
            request.Stock,
            request.ImageUrl,
            request.Rating,
            request.Size,
            request.Weight,
            request.Color,
            request.Specifications,
            request.BatteryCapacity,
            request.CameraSpecifications,
            request.Memory,
            request.StorageCapacity,
            request.Brand,
            request.ModelVersion,
            request.OperatingSystem);

        var result = await commandDispatcher.Dispatch<UpdateProductCommand, ProductDto>(command, cancellationToken);
        return Ok(result);
    }

    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        await commandDispatcher.Dispatch<DeleteProductCommand, bool>(new DeleteProductCommand(id), cancellationToken);
        return NoContent();
    }

    public record UpdateProductRequest(
        string Name,
        string Description,
        decimal Price,
        int Stock,
        string ImageUrl,
        decimal Rating,
        string Size,
        decimal Weight,
        string Color,
        Dictionary<string, string>? Specifications,
        string? BatteryCapacity,
        string? CameraSpecifications,
        string? Memory,
        string? StorageCapacity,
        string? Brand,
        string? ModelVersion,
        string? OperatingSystem);
}
