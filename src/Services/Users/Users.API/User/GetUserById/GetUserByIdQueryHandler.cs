using Users.API.Exceptions;

namespace Users.API.User.GetUserById
{
    public record GetUserByIdQuery(Guid UserId) : IQuery<UserDto>;
    public class GetUserByIdQueryHandler(IUserService userService,ILogger<GetUserByIdQueryHandler> logger)   
        : IQueryHandler<GetUserByIdQuery, UserDto>
    {
        public async Task<UserDto> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
        {
            logger.LogInformation("Handling GetUserByIdQuery for UserId: {UserId}", request.UserId);
            if (request.UserId == Guid.Empty)
            {
                logger.LogWarning("Invalid UserId provided: {UserId}", request.UserId);
                throw new ArgumentException("UserId cannot be empty.", nameof(request.UserId));
            }
            var user = await userService.GetUserByIdAsync(request.UserId, cancellationToken);

            if (user == null)
            {
                logger.LogWarning("User not found for UserId: {UserId}", request.UserId);
                throw new NotFoundException($"User with ID {request.UserId} not found.");
            }

            logger.LogInformation("User found: {@User}", user);

            return user;
        }
    }
}
