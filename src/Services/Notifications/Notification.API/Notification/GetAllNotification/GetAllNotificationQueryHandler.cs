
using Mapster;

namespace Notification.API.Notification.GetAllNotification
{
    public record GetAllNotificationQuery(Guid userId) : IQuery<IEnumerable<NotificationResponseDto>>;
    public class GetAllNotificationQueryHandler(IDocumentSession session , ILogger<GetAllNotificationQueryHandler>logger) : 
        IQueryHandler<GetAllNotificationQuery, IEnumerable<NotificationResponseDto>>
    {
        public async Task<IEnumerable<NotificationResponseDto>> Handle(GetAllNotificationQuery request, CancellationToken cancellationToken)
        {
            var notifications = await session.Query<Notifications>()
                .Where(n => n.UserId == request.userId)
                .OrderByDescending(x=>x.CreatedAt)
                .ToListAsync(cancellationToken);

            if (notifications == null || !notifications.Any())
            {
                logger.LogInformation("Not found any Notification for this user");
                return Enumerable.Empty<NotificationResponseDto>();
            }
            return notifications.Adapt<IEnumerable<NotificationResponseDto>>();
        }
    }
}
