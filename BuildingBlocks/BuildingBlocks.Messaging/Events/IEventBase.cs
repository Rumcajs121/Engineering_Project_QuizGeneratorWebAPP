namespace BuildingBlocks.Messaging.Events;

public interface IEventBase
{
    public Guid EventId { get;init;}
    public DateTime OccurredOnUtc{ get; init;}
    public string? EventType { get; }
}