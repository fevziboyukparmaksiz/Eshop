namespace Catalog.Contracts.Products.Dtos;

public record ProductDto(
    Guid Id,
    string Name,
    List<string> Category,
    string Description,
    decimal Price,
    string ImageFile);
