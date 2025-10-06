using BuildingBlocks.Messaging.Events;

namespace QuizService.Domain.Abstraction;

public abstract record DomainEvent:IEventBase
{
    public Guid EventId { get; init; }= Guid.NewGuid();
    public DateTime OccurredOnUtc { get; init; } = DateTime.UtcNow;
    public string EventType => GetType().AssemblyQualifiedName?? "";
};