namespace Users.API.User.LoginUser
{
    public class LoginEndpoints : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPost("/api/users/login", async (LoginDto loginDto, ISender sender) =>
            {
                var response = await sender.Send(new LoginCommand(loginDto));
                if (response is null)
                {
                    return Results.Problem("Invalid username or password.", statusCode: StatusCodes.Status401Unauthorized);
                }
                return Results.Ok(response);
            }).Accepts<LoginDto>("application/json")
              .Produces<AuthenticationResponse>(StatusCodes.Status200OK)
              .Produces(StatusCodes.Status401Unauthorized)
              .WithName("LoginUser")
              .WithTags("Users");
        }
    }
}
