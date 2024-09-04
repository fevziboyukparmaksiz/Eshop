namespace Basket.Dtos;

public record ShoppingCartDto(
    Guid Id,
    string Username,
    List<ShoppingCartItemDto> Items);


