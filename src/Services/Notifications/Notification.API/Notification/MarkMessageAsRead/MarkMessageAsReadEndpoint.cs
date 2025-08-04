
using MediatR;

namespace Notification.API.Notification.MarkMessageAsRead
{
    public class MarkMessageAsReadEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPatch("/api/notifications/{id:guid}/read", async (Guid id, ISender sender) =>
            {
                var response = await sender.Send(new MarkMessageAsReadCommand(id));

                return response
                    ? Results.Ok(true)
                    : Results.NotFound($"Notification with Id = {id} not found.");
            })
            .WithName("MarkAsReadSpecificMessage")
            .WithSummary("Mark a single notification as read by notification ID")
            .WithTags("Notifications");
        }
    }
}
