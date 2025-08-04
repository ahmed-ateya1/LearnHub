using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Notification.API.Notification.GetAllNotification
{
    public class GetAllNotificationEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/api/notifications/{userId:guid}", async (Guid userId,ISender sender) =>
            {
                if (userId == Guid.Empty)
                    return Results.BadRequest("User ID is required.");

                var response = await sender.Send(new GetAllNotificationQuery(userId));

                return response.Any()
                    ? Results.Ok(response)
                    : Results.NotFound("No notifications found for the specified user.");
            })
             .WithName("GetAllNotifications")
             .WithSummary("Retrieve all notifications by user ID")
             .WithTags("Notifications");

        }
    }
}
