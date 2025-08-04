namespace Users.API.Services.Contracts;
public record UserRegisteredEvent(Guid UserId, string Email, string FirstName,string LastName, DateTime CreatedAt);
