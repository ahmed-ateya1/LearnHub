using Carter;
using MediatR;

namespace Enrollement.API.Enrollement.GetEnrollementsByUser
{
    public class GetEnrollementByUserEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/api/enrollement/user/{userId:guid}", async (Guid userId, ISender sender) =>
            {
                var query = new GetEnrollementsByUserQuery(userId);
                var result = await sender.Send(query);
                return Results.Ok(result);
            })
            .WithTags("Enrollement")
            .WithName("GetEnrollementsByUser")
            .Produces<List<Models.Enrollement>>(StatusCodes.Status200OK);
        }
    }
}
