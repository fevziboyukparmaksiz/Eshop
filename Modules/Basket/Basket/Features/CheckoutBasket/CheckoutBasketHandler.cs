using System.Text.Json;
using Basket.Data;
using Basket.Dtos;
using Basket.Exceptions;
using Basket.Models;
using FluentValidation;
using Mapster;
using Microsoft.EntityFrameworkCore;
using Shared.Contracts.CQRS;
using Shared.Messaging.Events;

namespace Basket.Features.CheckoutBasket;

public record CheckoutBasketCommand(BasketCheckoutDto BasketCheckout) : ICommand<CheckoutBasketResult>;
public record CheckoutBasketResult(bool IsSuccess);
public class CheckoutBasketCommandValidator : AbstractValidator<CheckoutBasketCommand>
{
    public CheckoutBasketCommandValidator()
    {
        RuleFor(x => x.BasketCheckout).NotNull().WithMessage("BasketCheckoutDto can't be null");
        RuleFor(x => x.BasketCheckout.UserName).NotEmpty().WithMessage("UserName is required");
    }
}
public class CheckoutBasketHandler(BasketDbContext dbContext)
    : ICommandHandler<CheckoutBasketCommand,CheckoutBasketResult>
{
    public async Task<CheckoutBasketResult> Handle(CheckoutBasketCommand command, CancellationToken cancellationToken)
    {
        // get existing basket with total price
        // Set totalprice on basketcheckout event message
        // send basket checkout event to rabbitmq using masstransit
        // delete the basket

        await using var transaction =
            await dbContext.Database.BeginTransactionAsync(cancellationToken);

        try
        {
            // Get existing basket with total price
            var basket = await dbContext.ShoppingCarts
                .Include(x => x.Items)
                .SingleOrDefaultAsync(x => x.Username == command.BasketCheckout.UserName,cancellationToken);

            if (basket is null)
            {
                throw new BasketNotFoundException(command.BasketCheckout.UserName);
            }

            // Set total price on basket checkout event message

            var eventMessage = command.BasketCheckout.Adapt<BasketCheckoutIntegrationEvent>();
            eventMessage.TotalPrice = basket.TotalPrice;

            // Write a message to the outbox
            var outBoxMessage = new OutboxMessage()
            {
                Id = new Guid(),
                Type = typeof(BasketCheckoutIntegrationEvent).AssemblyQualifiedName,
                Content = JsonSerializer.Serialize(eventMessage),
                OccuredOn = DateTime.UtcNow
            };

            dbContext.OutboxMessages.Add(outBoxMessage);

            // Delete the basket
            dbContext.ShoppingCarts.Remove(basket);

            await dbContext.SaveChangesAsync(cancellationToken);
            await transaction.CommitAsync(cancellationToken);

            return new CheckoutBasketResult(true);
        }
        catch
        {
            await transaction.RollbackAsync(cancellationToken);
            return new CheckoutBasketResult(false);
        }

        ///////////////////// CHECKOUT BASKET WITHOUT OUTBOX
        //var basket =
        //    await repository.GetBasket(command.BasketCheckout.UserName, true, cancellationToken);
        //var eventMessage = command.BasketCheckout.Adapt<BasketCheckoutIntegrationEvent>();
        //eventMessage.TotalPrice = basket.TotalPrice;
        //await bus.Publish(eventMessage, cancellationToken);
        //await repository.DeleteBasket(command.BasketCheckout.UserName, cancellationToken);
        //return new CheckoutBasketResult(true);
        ///////////////////// CHECKOUT BASKET WITHOUT OUTBOX
    }
}
