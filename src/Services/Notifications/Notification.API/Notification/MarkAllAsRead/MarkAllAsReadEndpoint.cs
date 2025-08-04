
using MediatR;

namespace Notification.API.Notification.MarkAllAsRead
{
    public class MarkAllAsReadEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPatch("/api/notifications/{userId:guid}/read-all", async (Guid userId, ISender sender) =>
            {
                if (userId == Guid.Empty)
                    return Results.BadRequest("User ID is required.");

                var response = await sender.Send(new MarkAllAsReadCommand(userId));

                return response
                    ? Results.Ok(true)
                    : Results.NotFound($"No unread notifications found for user {userId}.");
            })
           .WithName("MarkAllAsRead")
           .WithSummary("Mark all notifications as read for a specific user")
           .WithTags("Notifications");
        }
    }
}
