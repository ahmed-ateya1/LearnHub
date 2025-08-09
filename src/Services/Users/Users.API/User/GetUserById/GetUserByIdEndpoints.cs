namespace Users.API.User.GetUserById
{
    public class GetUserByIdEndpoints : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/api/users/{userId:guid}", async (Guid userId, ISender sender) =>
            {
                var user = await sender.Send(new GetUserByIdQuery(userId));
                return user is not null ? Results.Ok(user) : Results.NotFound();
            }).Accepts<GetUserByIdQuery>("application/json")
             .Produces<UserDto>(StatusCodes.Status200OK)
             .Produces(StatusCodes.Status404NotFound)
             .WithName("GetUserById")
             .WithSummary("Retrieves a user by their ID.")
             .WithDescription("This endpoint allows you to retrieve a user by their unique identifier (ID).")
             .WithTags("Users");
        }
    }
}
