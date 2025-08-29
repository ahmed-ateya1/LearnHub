using BuildingBlocks.Messaging.Events;
using Mapster;
using Microsoft.AspNetCore.SignalR;
using Notification.API.Hubs;

namespace Notification.API.Consumers
{
    public class OrderPlacedConsumer(IDocumentSession session , IHubContext<NotificationHub> hubContext)
        : IConsumer<OrderPlacedEvent>
    {
        public async Task Consume(ConsumeContext<OrderPlacedEvent> context)
        {
            var message = context.Message;

            var notification = new Notifications
            {
                UserId = message.userId,
                Message = $"Your order with ID {message.orderId} has been placed successfully.",
                CreatedAt = DateTime.UtcNow,
                Type = "OrderPlaced",
                Id = Guid.NewGuid()
            };
            session.Store(notification);

            await session.SaveChangesAsync();

            var notificationDto = notification.Adapt<NotificationResponseDto>();

            await hubContext.Clients.User(message.userId.ToString())
                     .SendAsync("ReceiveNotification", notificationDto);

        }
    }
}
