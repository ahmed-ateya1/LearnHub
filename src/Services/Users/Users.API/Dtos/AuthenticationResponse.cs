using System.Text.Json.Serialization;

namespace Users.API.Dtos
{
    public record AuthenticationResponse(
             string Message,
             bool IsAuthenticated,
             string Email,
             Guid UserID,
             string FullName,
             List<string> Roles,
             string Token,
             string RefreshToken,
             DateTime RefreshTokenExpiration
        )
    {
        [JsonIgnore]
        public string RefreshToken { get; init; } = RefreshToken;
    }
}
