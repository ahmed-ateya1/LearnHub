namespace BuildingBlocks.Messaging.Events
{
    public record UserRegisteredEvent(Guid UserId, string Email, string FirstName, string LastName, DateTime CreatedAt);

}
