namespace Basket.Dtos;

public record ShoppingCartItemDto(
    Guid Id,
    Guid ShoppingCartId,
    Guid ProductId,
    int Quantity,
    decimal Price,
    string ProductName);

