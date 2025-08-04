
namespace Notification.API.Notification.DeleteNotification
{
    public record DeleteNotificationCommand(Guid Id): ICommand<bool>;
    public class DeleteNotificationCommandHandler(IDocumentSession session , ILogger<DeleteNotificationCommandHandler> logger)  
        : ICommandHandler<DeleteNotificationCommand, bool>
    {
        public async Task<bool> Handle(DeleteNotificationCommand request, CancellationToken cancellationToken)
        {
            var notification = await session.Query<Notifications>()
                .FirstOrDefaultAsync(x => x.Id == request.Id);

            if (notification == null)
                return false;

            session.Delete(notification);

            await session.SaveChangesAsync(cancellationToken);

            return true;

        }
    }
}
