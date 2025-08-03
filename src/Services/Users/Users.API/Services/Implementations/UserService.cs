using Users.API.Data;
using Users.API.Services.Contracts;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Users.API.Services.Implementations
{
    public class UserService(
        UserDbContext db,
        ILogger<UserService> logger,
        UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager,
        IWebHostEnvironment environment,
        IHttpContextAccessor httpContextAccessor,
        IConfiguration configuration) : IUserService
    {
        private readonly string _jwtSecret = configuration["JwtSettings:SecretKey"] ?? "your-super-secret-key-that-is-at-least-32-characters-long!";
        private readonly string _jwtIssuer = configuration["JwtSettings:Issuer"] ?? "YourApp";
        private readonly string _jwtAudience = configuration["JwtSettings:Audience"] ?? "YourAppUsers";
        private readonly int _jwtExpiryInMinutes = int.Parse(configuration["JwtSettings:ExpiryInMinutes"] ?? "60");
        private readonly int _refreshTokenExpiryInDays = int.Parse(configuration["JwtSettings:RefreshTokenExpiryInDays"] ?? "7");

        public async Task<UserDto> GetUserByIdAsync(Guid userId, CancellationToken cancellationToken = default)
        {
            var user = await db.Users.FindAsync(userId, cancellationToken);

            if (user == null)
                throw new KeyNotFoundException($"User with ID {userId} not found.");

            return new UserDto(
                user.Id,
                user.FirstName,
                user.LastName,
                user.DateOfBirth,
                user.Email!,
                string.Empty, 
                user.ProfilePictureUrl ?? string.Empty,
                user.Bio ?? string.Empty
            );
        }

        public async Task<AuthenticationResponse> RegisterAsync(RegisterDto registerDto, CancellationToken cancellationToken = default)
        {
            try
            {
                var existingUser = await userManager.FindByEmailAsync(registerDto.Email);
                if (existingUser != null)
                {
                    return new AuthenticationResponse(
                        "User with this email already exists.",
                        false,
                        string.Empty,
                        Guid.Empty,
                        string.Empty,
                        new List<string>(),
                        string.Empty,
                        string.Empty,
                        DateTime.MinValue
                    );
                }

                var picture = await CreateFile(registerDto.ProfilePictureUrl);
                
                var user = new ApplicationUser
                {
                    Id = Guid.NewGuid(),
                    FirstName = registerDto.FirstName,
                    LastName = registerDto.LastName,
                    DateOfBirth = DateTime.SpecifyKind(registerDto.DateOfBirth, DateTimeKind.Utc),
                    Email = registerDto.Email,
                    UserName = registerDto.Email,
                    Bio = registerDto.Bio,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                    ProfilePictureUrl = picture
                };

                var result = await userManager.CreateAsync(user, registerDto.Password);

                if (!result.Succeeded)
                {
                    var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                    return new AuthenticationResponse(
                        $"Registration failed: {errors}",
                        false,
                        string.Empty,
                        Guid.Empty,
                        string.Empty,
                        new List<string>(),
                        string.Empty,
                        string.Empty,
                        DateTime.MinValue
                    );
                }

                // Generate JWT token and refresh token
                var jwtToken = await GenerateJwtTokenAsync(user);
                var refreshToken = GenerateRefreshToken();

                // Save refresh token to database
                await SaveRefreshTokenAsync(user.Id, refreshToken);

                var roles = await userManager.GetRolesAsync(user);

                return new AuthenticationResponse(
                    "Registration successful.",
                    true,
                    user.Email!,
                    user.Id,
                    $"{user.FirstName} {user.LastName}",
                    roles.ToList(),
                    jwtToken,
                    refreshToken.Token,
                    refreshToken.ExpiresOn
                );
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error occurred during user registration");
                return new AuthenticationResponse(
                    "An error occurred during registration.",
                    false,
                    string.Empty,
                    Guid.Empty,
                    string.Empty,
                    new List<string>(),
                    string.Empty,
                    string.Empty,
                    DateTime.MinValue
                );
            }
        }

        public async Task<AuthenticationResponse> LoginAsync(LoginDto loginDto, CancellationToken cancellationToken = default)
        {
            try
            {
                var user = await userManager.FindByEmailAsync(loginDto.Email);
                if (user == null)
                {
                    return new AuthenticationResponse(
                        "Invalid email or password.",
                        false,
                        string.Empty,
                        Guid.Empty,
                        string.Empty,
                        new List<string>(),
                        string.Empty,
                        string.Empty,
                        DateTime.MinValue
                    );
                }

                var signInResult = await signInManager.CheckPasswordSignInAsync(user, loginDto.Password, false);

                if (!signInResult.Succeeded)
                {
                    return new AuthenticationResponse(
                        "Invalid email or password.",
                        false,
                        string.Empty,
                        Guid.Empty,
                        string.Empty,
                        new List<string>(),
                        string.Empty,
                        string.Empty,
                        DateTime.MinValue
                    );
                }

                // Generate JWT token and refresh token
                var jwtToken = await GenerateJwtTokenAsync(user);
                var refreshToken = GenerateRefreshToken();

                // Revoke old refresh tokens and save new one
                await RevokeAllUserRefreshTokensAsync(user.Id);
                await SaveRefreshTokenAsync(user.Id, refreshToken);

                var roles = await userManager.GetRolesAsync(user);

                return new AuthenticationResponse(
                    "Login successful.",
                    true,
                    user.Email!,
                    user.Id,
                    $"{user.FirstName} {user.LastName}",
                    roles.ToList(),
                    jwtToken,
                    refreshToken.Token,
                    refreshToken.ExpiresOn
                );
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error occurred during user login");
                return new AuthenticationResponse(
                    "An error occurred during login.",
                    false,
                    string.Empty,
                    Guid.Empty,
                    string.Empty,
                    new List<string>(),
                    string.Empty,
                    string.Empty,
                    DateTime.MinValue
                );
            }
        }

        public async Task<AuthenticationResponse> RefreshTokenAsync(string token)
        {
            try
            {
                var refreshToken = await db.Set<RefreshToken>()
                    .Include(rt => rt.User)
                    .FirstOrDefaultAsync(rt => rt.Token == token);

                if (refreshToken == null || !refreshToken.IsActive)
                {
                    return new AuthenticationResponse(
                        "Invalid or expired refresh token.",
                        false,
                        string.Empty,
                        Guid.Empty,
                        string.Empty,
                        new List<string>(),
                        string.Empty,
                        string.Empty,
                        DateTime.MinValue
                    );
                }

                // Revoke the used refresh token
                refreshToken.RevokedOn = DateTime.UtcNow;

                // Generate new tokens
                var jwtToken = await GenerateJwtTokenAsync(refreshToken.User);
                var newRefreshToken = GenerateRefreshToken();

                // Save new refresh token
                await SaveRefreshTokenAsync(refreshToken.UserId, newRefreshToken);
                await db.SaveChangesAsync();

                var roles = await userManager.GetRolesAsync(refreshToken.User);

                return new AuthenticationResponse(
                    "Token refreshed successfully.",
                    true,
                    refreshToken.User.Email!,
                    refreshToken.User.Id,
                    $"{refreshToken.User.FirstName} {refreshToken.User.LastName}",
                    roles.ToList(),
                    jwtToken,
                    newRefreshToken.Token,
                    newRefreshToken.ExpiresOn
                );
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error occurred during token refresh");
                return new AuthenticationResponse(
                    "An error occurred during token refresh.",
                    false,
                    string.Empty,
                    Guid.Empty,
                    string.Empty,
                    new List<string>(),
                    string.Empty,
                    string.Empty,
                    DateTime.MinValue
                );
            }
        }

        public async Task<bool> RevokeTokenAsync(string token)
        {
            try
            {
                var refreshToken = await db.Set<RefreshToken>()
                    .FirstOrDefaultAsync(rt => rt.Token == token);

                if (refreshToken == null || !refreshToken.IsActive)
                    return false;

                refreshToken.RevokedOn = DateTime.UtcNow;
                await db.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error occurred during token revocation");
                return false;
            }
        }

        private async Task<string> GenerateJwtTokenAsync(ApplicationUser user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_jwtSecret);

            var claims = new List<Claim>
            {
                new(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new(ClaimTypes.Email, user.Email!),
                new(ClaimTypes.Name, $"{user.FirstName} {user.LastName}"),
                new("firstName", user.FirstName),
                new("lastName", user.LastName)
            };

            // Add role claims
            var roles = await userManager.GetRolesAsync(user);
            claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(_jwtExpiryInMinutes),
                Issuer = _jwtIssuer,
                Audience = _jwtAudience,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        private RefreshToken GenerateRefreshToken()
        {
            using var rng = RandomNumberGenerator.Create();
            var randomBytes = new byte[64];
            rng.GetBytes(randomBytes);

            return new RefreshToken
            {
                Token = Convert.ToBase64String(randomBytes),
                ExpiresOn = DateTime.UtcNow.AddDays(_refreshTokenExpiryInDays),
                CreatedOn = DateTime.UtcNow
            };
        }

        private async Task SaveRefreshTokenAsync(Guid userId, RefreshToken refreshToken)
        {
            refreshToken.UserId = userId;
            db.Set<RefreshToken>().Add(refreshToken);
            await db.SaveChangesAsync();
        }

        private async Task RevokeAllUserRefreshTokensAsync(Guid userId)
        {
            var userRefreshTokens = await db.Set<RefreshToken>()
                .Where(rt => rt.UserId == userId && rt.RevokedOn == null)
                .ToListAsync();

            foreach (var token in userRefreshTokens)
            {
                token.RevokedOn = DateTime.UtcNow;
            }

            await db.SaveChangesAsync();
        }

        public async Task<string> CreateFile(IFormFile file)
        {
            try
            {
                string newFileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                string newPath = Path.Combine(environment.WebRootPath, "Upload", newFileName);

                if (!Directory.Exists(Path.Combine(environment.WebRootPath, "Upload")))
                {
                    Directory.CreateDirectory(Path.Combine(environment.WebRootPath, "Upload"));
                }

                using (var stream = new FileStream(newPath, FileMode.Create))
                {
                    await file.CopyToAsync(stream).ConfigureAwait(false);
                }

                var baseUrl = GetBaseUrl() + "Upload/" + newFileName;
                return baseUrl;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Error creating file", ex);
            }
        }
        private string GetBaseUrl()
        {
            var request = httpContextAccessor.HttpContext.Request;
            return $"{request.Scheme}://{request.Host.Value}/";
        }
    }
}