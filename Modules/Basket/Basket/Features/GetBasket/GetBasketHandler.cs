using Basket.Data;
using Basket.Data.Repositories;
using Basket.Dtos;
using Basket.Exceptions;
using Mapster;
using Microsoft.EntityFrameworkCore;
using Shared.CQRS;

namespace Basket.Features.GetBasket;

public record GetBasketQuery(string Username) : IQuery<GetBasketResult>;
public record GetBasketResult(ShoppingCartDto ShoppingCart);

public class GetBasketHandler(IBasketRepository basketRepository) 
    : IQueryHandler<GetBasketQuery,GetBasketResult>
{
    public async Task<GetBasketResult> Handle(GetBasketQuery query, CancellationToken cancellationToken)
    {
        var basket = await basketRepository.GetBasket(query.Username, true, cancellationToken);

        var basketDto = basket.Adapt<ShoppingCartDto>();

        return new GetBasketResult(basketDto);
    }
}
