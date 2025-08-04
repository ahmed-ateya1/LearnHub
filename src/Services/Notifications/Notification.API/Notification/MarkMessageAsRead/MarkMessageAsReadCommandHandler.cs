namespace Notification.API.Notification.MarkMessageAsRead
{
    public record MarkMessageAsReadCommand(Guid notificationId) : ICommand<bool>;
    public class MarkMessageAsReadCommandHandler(IDocumentSession session, ILogger<MarkMessageAsReadCommandHandler> logger)
        : ICommandHandler<MarkMessageAsReadCommand, bool>
    {
        public async Task<bool> Handle(MarkMessageAsReadCommand request, CancellationToken cancellationToken)
        {
            logger.LogInformation("Marking notification {NotificationId} as read", request.notificationId);

                // Load the notification document
            var notification = await session.LoadAsync<Notifications>
                (request.notificationId, cancellationToken);

            if (notification == null)
            {
                logger.LogWarning("Notification {NotificationId} not found", request.notificationId);
                return false;
            }

            notification.IsRead = true;
            notification.ReadAt = DateTime.UtcNow;

            session.Update(notification);

            await session.SaveChangesAsync(cancellationToken);

            logger.LogInformation("Successfully marked notification {NotificationId} as read", request.notificationId);
            return true;
        }
    }
}