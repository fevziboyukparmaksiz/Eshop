using Catalog.Products.Models;
using Shared.DDD;

namespace Catalog.Products.Events;

public record ProductCreatedEvent(Product Product) : IDomainEvent;

