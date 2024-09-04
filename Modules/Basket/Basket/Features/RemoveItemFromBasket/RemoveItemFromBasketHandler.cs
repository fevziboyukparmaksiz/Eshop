using Basket.Data;
using Basket.Exceptions;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Shared.CQRS;

namespace Basket.Features.RemoveItemFromBasket;

public record RemoveItemFromBasketCommand(string Username, Guid ProductId)
    : ICommand<RemoveItemFromBasketResult>;

public record RemoveItemFromBasketResult(Guid ShoppingCartId);
public class RemoveItemFromBasketCommandValidator : AbstractValidator<RemoveItemFromBasketCommand>
{
    public RemoveItemFromBasketCommandValidator()
    {
        RuleFor(x => x.Username).NotEmpty().WithMessage("UserName is required");
        RuleFor(x => x.ProductId).NotEmpty().WithMessage("ProductId is required");
    }
}
public class RemoveItemFromBasketHandler(BasketDbContext dbContext)
    :ICommandHandler<RemoveItemFromBasketCommand,RemoveItemFromBasketResult>
{
    public async Task<RemoveItemFromBasketResult> Handle(RemoveItemFromBasketCommand command, CancellationToken cancellationToken)
    {
        var shoppingCart = await dbContext.ShoppingCarts
            .Include(s => s.Items)
            .SingleOrDefaultAsync(s => s.Username == command.Username, cancellationToken);

        if (shoppingCart is null)
        {
            throw new BasketNotFoundException(command.Username);
        }

        var itemToRemove = shoppingCart.Items.SingleOrDefault(s => s.ProductId == command.ProductId);

        if (itemToRemove != null)
        {
            shoppingCart.RemoveItem(command.ProductId);
            await dbContext.SaveChangesAsync(cancellationToken);
        }

        return new RemoveItemFromBasketResult(shoppingCart.Id);
    }
}
