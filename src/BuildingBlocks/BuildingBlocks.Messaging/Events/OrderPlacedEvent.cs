namespace BuildingBlocks.Messaging.Events
{
    public record OrderPlacedEvent(Guid orderId , Guid userId, string orderStatus, DateTime createdAt);
}
