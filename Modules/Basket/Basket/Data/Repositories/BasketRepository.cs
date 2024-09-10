using Basket.Data;
using Basket.Exceptions;
using Basket.Models;
using Microsoft.EntityFrameworkCore;

namespace Basket.Data.Repositories;
public class BasketRepository(BasketDbContext dbContext)
    : IBasketRepository
{
    public async Task<ShoppingCart> GetBasket(string username, bool asNoTracking, CancellationToken cancellationToken = default)
    {
        var query = dbContext.ShoppingCarts
            .Include(x => x.Items)
            .Where(x => x.Username == username);

        if (asNoTracking)
        {
            query = query.AsNoTracking();
        }

        var basket = await query.SingleOrDefaultAsync(cancellationToken);

        return basket ?? throw new BasketNotFoundException(username);
    }

    public async Task<ShoppingCart> CreateBasket(ShoppingCart shoppingCart, CancellationToken cancellationToken = default)
    {
        dbContext.ShoppingCarts.Add(shoppingCart);
        await dbContext.SaveChangesAsync(cancellationToken);

        return shoppingCart;
    }

    public async Task<bool> DeleteBasket(string username, CancellationToken cancellationToken = default)
    {
        var basket = await GetBasket(username, false, cancellationToken);

        dbContext.ShoppingCarts.Remove(basket);
        await dbContext.SaveChangesAsync(cancellationToken);

        return true;
    }

    public async Task<int> SaveChangesAsync(string? username = null,CancellationToken cancellationToken = default )
    {
        return await dbContext.SaveChangesAsync(cancellationToken);
    }
}
