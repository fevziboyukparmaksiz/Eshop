using Basket.Data.Repositories;
using FluentValidation;
using Shared.Contracts.CQRS;

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
public class RemoveItemFromBasketHandler(IBasketRepository basketRepository)
    :ICommandHandler<RemoveItemFromBasketCommand,RemoveItemFromBasketResult>
{
    public async Task<RemoveItemFromBasketResult> Handle(RemoveItemFromBasketCommand command, CancellationToken cancellationToken)
    {
        var shoppingCart = await basketRepository.GetBasket(command.Username, false, cancellationToken);

        shoppingCart.RemoveItem(command.ProductId);
        await basketRepository.SaveChangesAsync(command.Username,cancellationToken);
        
        return new RemoveItemFromBasketResult(shoppingCart.Id);
    }
}
