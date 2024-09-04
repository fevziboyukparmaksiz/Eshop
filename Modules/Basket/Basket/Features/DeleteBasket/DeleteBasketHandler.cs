using Basket.Data;
using Basket.Exceptions;
using Microsoft.EntityFrameworkCore;
using Shared.CQRS;

namespace Basket.Features.DeleteBasket;

public record DeleteBasketCommand(string Username) 
    : ICommand<DeleteBasketResult>;
public record DeleteBasketResult(bool IsSuccess);

public class DeleteBasketHandler(BasketDbContext dbContext) 
    : ICommandHandler<DeleteBasketCommand, DeleteBasketResult>
{
    public async Task<DeleteBasketResult> Handle(DeleteBasketCommand command, CancellationToken cancellationToken)
    {
        var shoppingCart =
            await dbContext.ShoppingCarts.SingleOrDefaultAsync(s => s.Username == command.Username, cancellationToken);

        if (shoppingCart is null)
        {
            throw new BasketNotFoundException(command.Username);
        }

        dbContext.ShoppingCarts.Remove(shoppingCart);
        await dbContext.SaveChangesAsync(cancellationToken);

        return new DeleteBasketResult(true);
    }
}
