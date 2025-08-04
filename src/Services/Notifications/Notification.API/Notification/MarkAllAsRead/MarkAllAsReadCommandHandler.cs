using Marten.Patching;

namespace Notification.API.Notification.MarkAllAsRead
{
    public record class MarkAllAsReadCommand(Guid userId): ICommand<bool>;
    public class MarkAllAsReadCommandHandler(IDocumentSession session, ILogger<MarkAllAsReadCommandHandler> logger)
     : ICommandHandler<MarkAllAsReadCommand, bool>
    {
        public async Task<bool> Handle(MarkAllAsReadCommand request, CancellationToken cancellationToken)
        {
            session.Patch<Notifications>(x => x.UserId == request.userId && !x.IsRead)
             .Set(x => x.IsRead, true)
             .Set(x => x.ReadAt, DateTime.UtcNow);



            await session.SaveChangesAsync(cancellationToken);

            logger.LogInformation("Marked all notifications as read for user {UserId}", request.userId);

            return true;
        }
    }

}
