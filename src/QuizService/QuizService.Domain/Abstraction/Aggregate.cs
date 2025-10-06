namespace QuizService.Domain.Abstraction;

public abstract class Aggregate<TId>:Entity<TId>,IAggregate<TId>
{
    private readonly List<DomainEvent> _domainEvents = new ();
    public IReadOnlyCollection<DomainEvent> DomainEvents =>_domainEvents.AsReadOnly();
    public void AddDomainEvent(DomainEvent domainEvent)
    {
        _domainEvents.Add(domainEvent);
    }
    public DomainEvent[] ClearDomainEvent()
    {
        DomainEvent[] dequeuedEvents = _domainEvents.ToArray();
        _domainEvents.Clear();
        return dequeuedEvents;
    }
}