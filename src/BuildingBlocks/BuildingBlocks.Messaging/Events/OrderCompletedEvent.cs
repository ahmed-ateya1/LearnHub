namespace BuildingBlocks.Messaging.Events
{
    public record OrderCompletedEvent(Guid orderId ,
        Guid userId, 
        string orderStatus,
        IEnumerable<Guid> courseIds,
        DateTime createdAt);
}
