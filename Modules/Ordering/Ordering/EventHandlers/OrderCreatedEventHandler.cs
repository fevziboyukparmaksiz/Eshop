using MediatR;
using Microsoft.Extensions.Logging;
using Ordering.Events;

namespace Ordering.EventHandlers;
public class OrderCreatedEventHandler(ILogger<OrderCreatedEventHandler> logger)
    : INotificationHandler<OrderCreatedEvent>
{
    public Task Handle(OrderCreatedEvent notification, CancellationToken cancellationToken)
    {
        logger.LogInformation("Domain Event handled : {DomainEvent}", notification.GetType().Name);
        return Task.CompletedTask;
    }
}
