using System.Text.Json;
using System.Text.Json.Serialization;
using Basket.Data.JsonConverters;
using Basket.Models;
using Microsoft.Extensions.Caching.Distributed;

namespace Basket.Data.Repositories;
public class CachedBasketRepository(
    IBasketRepository basketRepository,
    IDistributedCache cache)
    : IBasketRepository
{

    private readonly JsonSerializerOptions _options = new JsonSerializerOptions
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
        Converters = { new ShoppingCartConverter(), new ShoppingCartItemConverter() }
    };

    public async Task<ShoppingCart> GetBasket(string username, bool asNoTracking, CancellationToken cancellationToken = default)
    {
        if (!asNoTracking)
        {
            return await basketRepository.GetBasket(username, false, cancellationToken);
        }

        var cachedBasket = await cache.GetStringAsync(username, cancellationToken);

        if (!string.IsNullOrEmpty(cachedBasket))
        {
            var dd = JsonSerializer.Deserialize<ShoppingCart>(cachedBasket,_options)!;
            return dd;
        }

        var basket = await basketRepository.GetBasket(username, asNoTracking, cancellationToken);

        await cache.SetStringAsync(username, JsonSerializer.Serialize(basket,_options), cancellationToken);

        return basket;
    }

    public async Task<ShoppingCart> CreateBasket(ShoppingCart shoppingCart, CancellationToken cancellationToken = default)
    {
        await basketRepository.CreateBasket(shoppingCart, cancellationToken);

        await cache.SetStringAsync(shoppingCart.Username, JsonSerializer.Serialize(shoppingCart,_options), cancellationToken);

        return shoppingCart;
    }

    public async Task<bool> DeleteBasket(string username, CancellationToken cancellationToken = default)
    {
        await basketRepository.DeleteBasket(username, cancellationToken);

        await cache.RemoveAsync(username, cancellationToken);

        return true;
    }

    public async Task<int> SaveChangesAsync(string? username = null, CancellationToken cancellationToken = default)
    {
        var result = await basketRepository.SaveChangesAsync(username,cancellationToken);

        if (username is not null)
        {
            await cache.RemoveAsync(username, cancellationToken);
        }

        return result;
    }
}
