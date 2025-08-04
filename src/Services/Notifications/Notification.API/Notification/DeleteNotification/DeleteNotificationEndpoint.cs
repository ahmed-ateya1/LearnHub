using MediatR;

namespace Notification.API.Notification.DeleteNotification
{
    public class DeleteNotificationEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapDelete("/api/notifications/{id:guid}", async (Guid id, ISender sender) =>
            {
                var response = await sender.Send(new DeleteNotificationCommand(id));

                return response
                    ? Results.Ok(true)
                    : Results.NotFound($"Notification with Id = {id} not found.");
            })
            .WithName("DeleteNotification")
            .WithSummary("Delete a specific notification by ID")
            .WithTags("Notifications");
        }
    }
}
