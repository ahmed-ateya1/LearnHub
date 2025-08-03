
namespace Users.API.User.LoginUser
{
    public record LoginCommand(LoginDto LoginDto) : ICommand<AuthenticationResponse>;

    public class LoginCommandValidator : AbstractValidator<LoginCommand>
    {
        public LoginCommandValidator()
        {
            RuleFor(x => x.LoginDto.Email)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("Invalid email format.");
            RuleFor(x => x.LoginDto.Password)
                .NotEmpty().WithMessage("Password is required.")
                .MinimumLength(6).WithMessage("Password must be at least 6 characters long.");
        }
    }
    public class LoginCommandHandler(IUserService userService,ILogger<LoginCommandHandler>logger) : ICommandHandler<LoginCommand, AuthenticationResponse>
    {
        public async Task<AuthenticationResponse> 
            Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            logger.LogInformation("Handling login command for user: {Email}", request.LoginDto.Email);    
            var response = await userService.LoginAsync(request.LoginDto, cancellationToken);

            logger.LogInformation("Login command handled for user: {Email}", request.LoginDto.Email);
            if (response is null)
            {
                logger.LogWarning("Login failed for user: {Email}", request.LoginDto.Email);
                throw new UnauthorizedAccessException("Invalid username or password.");
            }
            logger.LogInformation("Login successful for user: {Email}", request.LoginDto.Email);
            return response;

        }
    }
}
