using Carter;
using MediatR;

namespace Enrollement.API.Enrollement.DeleteEnrollement
{
    public class DeleteEnrollementEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapDelete("/api/enrollement/{id:guid}", async (Guid id, ISender sender) =>
            {
                var command = new DeleteEnrollementCommand(id);
                var result = await sender.Send(command);
                if (!result)
                {
                    return Results.NotFound();
                }
                return Results.NoContent();
            })
            .WithTags("Enrollement")
            .WithName("DeleteEnrollement")
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status404NotFound);
        }
    }
}
