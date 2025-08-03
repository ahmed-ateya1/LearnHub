namespace Users.API.Dtos
{
    public record RegisterDto(string FirstName,
         string LastName,
         DateTime DateOfBirth,
         string Email,
         string Password,
         IFormFile? ProfilePictureUrl = null,
         string? Bio = null
     );
}
