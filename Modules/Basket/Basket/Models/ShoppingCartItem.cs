using Shared.DDD;

namespace Basket.Models;
public class ShoppingCartItem : Entity<Guid>
{
    public ShoppingCartItem(Guid shoppingCartId, Guid productId, int quantity, decimal price, string productName)
    {
        ShoppingCartId = shoppingCartId;
        ProductId = productId;
        Quantity = quantity;
        Price = price;
        ProductName = productName;
    }

    public Guid ShoppingCartId { get; private set; }
    public Guid ProductId { get; private set; }
    public int Quantity { get; set; }
    public decimal Price { get; private set; }
    public string ProductName { get; private set; }


}
