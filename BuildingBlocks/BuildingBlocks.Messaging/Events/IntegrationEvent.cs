namespace BuildingBlocks.Messaging.Events;

public abstract record IntegrationEvent:IEventBase
{
    public Guid EventId { get; init; }= Guid.NewGuid();
    public DateTime OccurredOnUtc { get; init; } = DateTime.Now;
    public string EventType => GetType().FullName ?? "";
    public abstract string EventName { get; }
    public abstract string Producer { get; } 
    public virtual string SchemaVersion => "1.0";
    public Guid? CorrelationId { get; init; }
    public Guid? CausationId { get; init; }
}