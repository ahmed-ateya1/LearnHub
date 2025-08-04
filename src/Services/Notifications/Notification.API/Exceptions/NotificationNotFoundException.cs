using BuildingBlocks.Exceptions;

namespace Notification.API.Exception
{
    public class NotificationNotFoundException : NotFoundException
    {
        public NotificationNotFoundException(string message) : base(message)
        {
        }
    }
}
