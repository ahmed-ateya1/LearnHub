using BuildingBlocks.Messaging.Events;
using Mapster;
using Microsoft.AspNetCore.SignalR;
using Notification.API.Hubs;

namespace Notification.API.Consumers
{
    public class CoursePublishedConsumer(IDocumentSession session, IHubContext<NotificationHub> hub)
        : IConsumer<CoursePublishedEvent>
    {
        public async Task Consume(ConsumeContext<CoursePublishedEvent> context)
        {
            var message = context.Message;

            var notification = new Notifications
            {
                UserId = message.InstructorId,
                Message = $"Congratulations! Your course '{message.Title}' has been published successfully.",
                CreatedAt = DateTime.UtcNow,
                Type = "CoursePublished",
                Id = Guid.NewGuid()
            };

            session.Store(notification);

            await session.SaveChangesAsync();

            var notificationDto = notification.Adapt<NotificationResponseDto>();

            await hub.Clients.User(message.InstructorId.ToString())
                     .SendAsync("ReceiveNotification", notificationDto);
        }
    }
}
