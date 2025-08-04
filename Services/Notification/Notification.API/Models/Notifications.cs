namespace Notification.API.Models
{
    public class Notifications
    {
        public Guid Id { get; set; }
        public string Message { get; set; }
        public bool IsRead { get; set; } = false;
        public string? Type { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? ReadAt { get; set; }
        public Guid UserId { get; set; }
    }
}
