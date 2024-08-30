namespace Shared.DDD;
public interface IAggreagate<T> : IEntity<T>, IAggreagate
{
}

public interface IAggreagate
{
    IReadOnlyList<IDomainEvent> DomainEvents { get; }
    IDomainEvent[] ClearDomainEvents();
}
