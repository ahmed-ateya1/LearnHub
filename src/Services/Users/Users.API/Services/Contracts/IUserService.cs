namespace Users.API.Services.Contracts
{
    public interface IUserService
    {
        public Task<AuthenticationResponse> RegisterAsync(
            RegisterDto registerDto,
            CancellationToken cancellationToken = default);

        public Task<AuthenticationResponse> LoginAsync(
            LoginDto loginDto,
            CancellationToken cancellationToken = default);


        public Task<UserDto> GetUserByIdAsync(
            Guid userId,
            CancellationToken cancellationToken = default);

        Task<AuthenticationResponse> RefreshTokenAsync(string token);

        Task<bool> RevokeTokenAsync(string token);


        Task<string> CreateFile(IFormFile file);
    }
}
