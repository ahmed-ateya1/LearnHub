using BuildingBlocks.Messaging.Events;
using Mapster;
using Microsoft.AspNetCore.SignalR;
using Notification.API.Dtos.Enums;
using Notification.API.Hubs;

namespace Notification.API.Consumers
{
    public class UserRegisteredConsumer(IDocumentSession session , IHubContext<NotificationHub> hub) : IConsumer<UserRegisteredEvent>
    {
        public async Task Consume(ConsumeContext<UserRegisteredEvent> context)
        {
            var message = context.Message;

            var notification = new Notifications
            {
                UserId = message.UserId,
                Message = $"Welcome {message.FirstName} {message.LastName}! Your account has been successfully created.",
                CreatedAt = DateTime.UtcNow,
                Type = NotificationType.Welcome.ToString(),
                Id = Guid.NewGuid()
            };

            session.Store(notification);

            await session.SaveChangesAsync();

            var notificationDto = notification.Adapt<NotificationResponseDto>();


            await hub.Clients.User(message.UserId.ToString())
                     .SendAsync("ReceiveNotification", notificationDto);

        }
    }
}
