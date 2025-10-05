namespace BuildingBlocks.Messaging.Events;

public interface IEventBase
{
    public Guid EventId { get;}
    public DateTime OccurredOnUtc{ get;}
    public string? EventType { get;}
}