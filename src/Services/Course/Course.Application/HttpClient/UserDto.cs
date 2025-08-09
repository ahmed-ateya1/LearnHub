namespace Course.Application.HttpClient;
public record UserDto(
        Guid Id,
        string FirstName,
        string LastName,
        DateTime DateOfBirth,
        string Email,
        string Password,
        string ProfilePictureUrl,
        string Bio);
