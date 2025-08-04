using Mapster;

namespace Notification.API.Notification.GetOnlyUnreadNotification
{ 
    public record GetOnlyUnReadCommand(Guid userId) : IQuery<IEnumerable<NotificationResponseDto>>;
    public class GetOnlyUnReadCommandHandler(IDocumentSession session , ILogger<GetOnlyUnReadCommand> logger) : 
        IQueryHandler<GetOnlyUnReadCommand, IEnumerable<NotificationResponseDto>>
    {
        public async Task<IEnumerable<NotificationResponseDto>> Handle(GetOnlyUnReadCommand request, CancellationToken cancellationToken)
        {
            var notifications = await session.Query<Notifications>()
                .Where(x => !x.IsRead)
                .OrderByDescending(x => x.CreatedAt)
                .ToListAsync();

            if(notifications == null|| !notifications.Any())
            {
                logger.LogInformation("Not found any Notification for this user");
                return Enumerable.Empty<NotificationResponseDto>();
            }

            return notifications.Adapt<IEnumerable<NotificationResponseDto>>();
        }
    }
}
