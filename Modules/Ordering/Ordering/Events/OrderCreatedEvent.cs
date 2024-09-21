using Ordering.Models;
using Shared.DDD;

namespace Ordering.Events;

public record OrderCreatedEvent(Order Order) : IDomainEvent;
