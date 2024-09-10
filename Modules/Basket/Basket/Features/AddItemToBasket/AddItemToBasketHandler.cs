using Basket.Data;
using Basket.Data.Repositories;
using Basket.Dtos;
using Basket.Exceptions;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Shared.CQRS;

namespace Basket.Features.AddItemToBasket;

public record AddItemToBasketCommand(string Username,ShoppingCartItemDto ShoppingCartItem) 
    :ICommand<AddItemToBasketResult>;
public record AddItemToBasketResult(Guid ShoppingCartId);

public class AddItemToBasketCommandValidator : AbstractValidator<AddItemToBasketCommand>
{
    public AddItemToBasketCommandValidator()
    {
        RuleFor(x => x.Username).NotEmpty().WithMessage("UserName is required");
        RuleFor(x => x.ShoppingCartItem.ProductId).NotEmpty().WithMessage("ProductId is required");
        RuleFor(x => x.ShoppingCartItem.Quantity).GreaterThan(0).WithMessage("Quantity must be greater than 0");
    }
}

public class AddItemToBasketHandler(IBasketRepository basketRepository) 
    : ICommandHandler<AddItemToBasketCommand,AddItemToBasketResult>
{
    public async Task<AddItemToBasketResult> Handle(AddItemToBasketCommand command, CancellationToken cancellationToken)
    {
        var shoppingCart = await basketRepository.GetBasket(command.Username, false, cancellationToken);
        
        shoppingCart.AddItem(
            command.ShoppingCartItem.ProductId,
            command.ShoppingCartItem.Quantity,
            command.ShoppingCartItem.ProductName,
            command.ShoppingCartItem.Price);

        await basketRepository.SaveChangesAsync(command.Username,cancellationToken);

        return new AddItemToBasketResult(shoppingCart.Id);
    }
}
