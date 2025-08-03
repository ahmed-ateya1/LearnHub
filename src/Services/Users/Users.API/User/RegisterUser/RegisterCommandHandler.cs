namespace Users.API.User.RegisterUser
{
    
    public record RegisterCommand(RegisterDto registerDto) : ICommand<AuthenticationResponse>;
    public class RegisterCommandHandler(IUserService userService,ILogger<RegisterCommandHandler> logger) 
        : ICommandHandler<RegisterCommand, AuthenticationResponse>
    {
        public async Task<AuthenticationResponse> Handle(RegisterCommand request, CancellationToken cancellationToken)
        {
            logger.LogInformation("Handling RegisterCommand for user registration");
            var result = await userService
                .RegisterAsync(request.registerDto, cancellationToken);

            logger.LogInformation("User registration completed successfully for {Email}", request.registerDto.Email);

            return result;
        }
    }
}
