using BuildingBlocks.Messaging.Events;
using MassTransit;

namespace Users.API.User.RegisterUser
{
    
    public record RegisterCommand(RegisterDto registerDto) : ICommand<AuthenticationResponse>;
    public class RegisterCommandValidator : AbstractValidator<RegisterCommand>
    {
        public RegisterCommandValidator()
        {
            RuleFor(x => x.registerDto)
                .NotNull()
                .WithMessage("RegisterDto cannot be null");
            RuleFor(x => x.registerDto.Email)
                .NotEmpty()
                .EmailAddress()
                .WithMessage("A valid email is required");
            RuleFor(x => x.registerDto.Password)
                .NotEmpty()
                .MinimumLength(6)
                .WithMessage("Password must be at least 6 characters long");
        }
    }
    public class RegisterCommandHandler(IUserService userService,IPublishEndpoint publish ,ILogger<RegisterCommandHandler> logger) 
        : ICommandHandler<RegisterCommand, AuthenticationResponse>
    {
        public async Task<AuthenticationResponse> Handle(RegisterCommand request, CancellationToken cancellationToken)
        {
            logger.LogInformation("Handling RegisterCommand for user registration");
            var result = await userService
                .RegisterAsync(request.registerDto, cancellationToken);

            await publish.Publish(new UserRegisteredEvent(
                result.UserID,
                request.registerDto.Email,
                request.registerDto.FirstName,
                request.registerDto.LastName,
                DateTime.UtcNow), cancellationToken);
            logger.LogInformation("User registration completed successfully for {Email}", request.registerDto.Email);

            return result;
        }
    }
}

