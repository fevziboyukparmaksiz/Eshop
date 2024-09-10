using Basket.Models;

namespace Basket.Data.Repositories;
public interface IBasketRepository
{
    Task<ShoppingCart> GetBasket(string username, bool asNoTracking = true, CancellationToken cancellationToken = default);
    Task<ShoppingCart> CreateBasket(ShoppingCart shoppingCart, CancellationToken cancellationToken = default);
    Task<bool> DeleteBasket(string username, CancellationToken cancellationToken = default);
    Task<int> SaveChangesAsync(string? userName = null, CancellationToken cancellationToken = default);
}
