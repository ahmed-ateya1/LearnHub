using Microsoft.AspNetCore.Mvc;

namespace Users.API.User.RegisterUser
{
    public class RegisterEndpoints : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPost("/api/users/register", async ([FromForm] RegisterDto register, ISender sender) =>
            {
                var result = await sender.Send(new RegisterCommand(register));
                if(result == null)
                {
                    return Results.BadRequest("Registration failed. Please check your input and try again.");
                }
                return Results.Ok(result);
            }).WithName("RegisterUser")
            .DisableAntiforgery()
             .WithTags("Authentication")
             .Produces<AuthenticationResponse>(StatusCodes.Status200OK)
             .ProducesValidationProblem(StatusCodes.Status400BadRequest)
             .WithSummary("Registers a new user and returns authentication tokens.")
             .WithDescription("Accepts user registration details, creates a new user, and returns JWT and refresh token for authentication.");

        }
    }
}
