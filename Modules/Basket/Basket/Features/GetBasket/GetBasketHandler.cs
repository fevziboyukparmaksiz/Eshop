using Basket.Data;
using Basket.Dtos;
using Basket.Exceptions;
using Mapster;
using Microsoft.EntityFrameworkCore;
using Shared.CQRS;

namespace Basket.Features.GetBasket;

public record GetBasketQuery(string Username) : IQuery<GetBasketResult>;
public record GetBasketResult(ShoppingCartDto ShoppingCart);

public class GetBasketHandler(BasketDbContext dbContext) 
    : IQueryHandler<GetBasketQuery,GetBasketResult>
{
    public async Task<GetBasketResult> Handle(GetBasketQuery query, CancellationToken cancellationToken)
    {
        var basket = await dbContext.ShoppingCarts
            .AsNoTracking()
            .Include(x => x.Items)
            .SingleOrDefaultAsync(x => x.Username == query.Username, cancellationToken);

        if (basket is null)
        {
            throw new BasketNotFoundException(query.Username);
        }

        var basketDto = basket.Adapt<ShoppingCartDto>();

        return new GetBasketResult(basketDto);
    }
}
