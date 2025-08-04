
using MediatR;

namespace Notification.API.Notification.GetOnlyUnreadNotification
{
    public class GetOnlyUnReadEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/api/notifications/{userId:guid}/unread", async (Guid userId, ISender sender) =>
            {
                if (userId == Guid.Empty)
                    return Results.BadRequest("User ID is required.");

                var response = await sender.Send(new GetOnlyUnReadCommand(userId));

                return response.Any()
                    ? Results.Ok(response)
                    : Results.NotFound("No unread notifications found for this user.");
            })
            .WithName("GetUnreadNotifications")
            .WithSummary("Retrieve only unread notifications by user ID")
            .WithTags("Notifications");
        }
    }
}
