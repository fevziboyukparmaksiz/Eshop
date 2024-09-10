using Basket.Data;
using Basket.Data.Repositories;
using Basket.Dtos;
using Basket.Models;
using FluentValidation;
using Shared.CQRS;

namespace Basket.Features.CreateBasket;

public record CreateBasketCommand(ShoppingCartDto ShoppingCart) 
    : ICommand<CreateBasketResult>;

public record CreateBasketResult(Guid Id);

public class CreateBasketCommandValidator : AbstractValidator<CreateBasketCommand>
{
    public CreateBasketCommandValidator()
    {
        RuleFor(b => b.ShoppingCart.Username).NotEmpty().WithMessage("Username is required");
    }
}

public class CreateBasketHandler(IBasketRepository basketRepository) 
    : ICommandHandler<CreateBasketCommand,CreateBasketResult>
{
    public async Task<CreateBasketResult> Handle(CreateBasketCommand command, CancellationToken cancellationToken)
    {
        var shoppingCart = CreateNewShoppingCart(command.ShoppingCart);

        await basketRepository.CreateBasket(shoppingCart, cancellationToken);

        return new CreateBasketResult(shoppingCart.Id);
    }

    private ShoppingCart CreateNewShoppingCart(ShoppingCartDto shoppingCartDto)
    {
        var newShoppingCart = ShoppingCart.Create(
            shoppingCartDto.Id,
            shoppingCartDto.Username);

        shoppingCartDto.Items.ForEach(item =>
        {
            newShoppingCart.AddItem(
                item.ProductId,
                item.Quantity,
                item.ProductName,
                item.Price);
        });

        return newShoppingCart;
    }
}


