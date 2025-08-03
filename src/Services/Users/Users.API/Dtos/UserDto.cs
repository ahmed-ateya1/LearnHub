namespace Users.API.Dtos
{
    public record UserDto(
         Guid Id,
         string FirstName,
         string LastName,
         DateTime DateOfBirth,
         string Email,
         string Password,
         string ProfilePictureUrl,
         string Bio);
}
