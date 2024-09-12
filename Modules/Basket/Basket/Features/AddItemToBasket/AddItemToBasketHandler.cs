using Basket.Data.Repositories;
using Basket.Dtos;
using Catalog.Contracts.Features.GetProductById;
using FluentValidation;
using MediatR;
using Shared.Contracts.CQRS;

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

public class AddItemToBasketHandler(
    IBasketRepository basketRepository, ISender sender) 
    : ICommandHandler<AddItemToBasketCommand,AddItemToBasketResult>
{
    public async Task<AddItemToBasketResult> Handle(AddItemToBasketCommand command, CancellationToken cancellationToken)
    {
        var shoppingCart = await basketRepository.GetBasket(command.Username, false, cancellationToken);
        
        //TODO: Before AddItem into SC, we should call Catalog Module GetProductById method
        // Get latest product information and set Price and ProductName when adding item into SC

        var result = await sender.Send(new GetProductByIdQuery(command.ShoppingCartItem.ProductId),cancellationToken);

        shoppingCart.AddItem(
            command.ShoppingCartItem.ProductId,
            command.ShoppingCartItem.Quantity,
            result.Product.Name,
            result.Product.Price);
            //command.ShoppingCartItem.Price,
            //command.ShoppingCartItem.ProductName);

        await basketRepository.SaveChangesAsync(command.Username,cancellationToken);

        return new AddItemToBasketResult(shoppingCart.Id);
    }
}
