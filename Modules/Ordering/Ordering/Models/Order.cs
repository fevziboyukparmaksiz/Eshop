using Ordering.Events;
using Ordering.ValueObjects;
using Shared.DDD;

namespace Ordering.Models;
public class Order : Aggreagate<Guid>
{
    private readonly List<OrderItem> _items = new();
    public IReadOnlyList<OrderItem> Items => _items.AsReadOnly();

    public Guid CustomerId { get; set; }
    public string OrderName { get; set; }
    public Address ShippingAddress { get; set; }
    public Address BillingAddress { get; set; }
    public Payment Payment { get; set; }
    public decimal TotalPrice => Items.Sum(x => x.Price*x.Quantity);

    public static Order Create(Guid id, Guid customerId, string orderName, Address shippingAddress, Address billingAddress, Payment payment)
    {
        var order = new Order()
        {
            Id = id,
            CustomerId = customerId,
            OrderName = orderName,
            ShippingAddress = shippingAddress,
            BillingAddress = billingAddress,
            Payment = payment
        };

        order.AddDomainEvent(new OrderCreatedEvent(order));

        return order;
    }

    public void Add(Guid productId, int quantity, decimal price)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(quantity);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(price);

        var existingItem = Items.FirstOrDefault(x => x.ProductId == productId);

        if (existingItem is not null)
        {
            existingItem.Quantity += quantity;
        }
        else
        {
            var orderItem = new OrderItem(Id, productId, quantity, price);
            _items.Add(orderItem);
        }

    }

    public void Remove(Guid productId)
    {
        var orderItem = Items.FirstOrDefault(x => x.ProductId == productId);

        if (orderItem is not null)
        {
            _items.Remove(orderItem);
        }
    }

}
