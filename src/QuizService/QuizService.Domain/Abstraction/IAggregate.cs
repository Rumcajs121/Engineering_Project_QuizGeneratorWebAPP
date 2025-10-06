namespace QuizService.Domain.Abstraction;

public interface IAggregate<T> : IAggregate, IEntity<T>
{
    
}
public interface IAggregate:IEntity
{
    IReadOnlyCollection<DomainEvent> DomainEvents { get; }
    DomainEvent[] ClearDomainEvent();
}