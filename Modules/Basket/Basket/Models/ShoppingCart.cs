using Shared.DDD;

namespace Basket.Models;
public class ShoppingCart : Aggreagate<Guid>
{
    public string Username { get; private set; } = default!;
    private readonly List<ShoppingCartItem> _items = new();
    public IReadOnlyList<ShoppingCartItem> Items => _items.AsReadOnly();
    public decimal TotalPrice => Items.Sum(x => x.Price * x.Quantity);

    public static ShoppingCart Create(Guid id, string username)
    {
        ArgumentException.ThrowIfNullOrEmpty(username);

        var shoppingCart = new ShoppingCart()
        {
            Id = id,
            Username = username
        };

        return shoppingCart;
    }

    public void AddItem(Guid productId, int quantity, string productName, decimal price)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(quantity);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(price);

        var existingItem = _items.FirstOrDefault(x => x.ProductId == productId);

        if (existingItem != null)
        {
            existingItem.Quantity += quantity;
        }
        else
        {
            var newItem = new ShoppingCartItem(Id, productId, quantity, price, productName);
            _items.Add(newItem);
        }

    }

    public void RemoveItem(Guid productId)
    {
        var existingItem = _items.FirstOrDefault(x => x.ProductId == productId);

        if (existingItem != null)
        {
            _items.Remove(existingItem);
        }
    }
}
