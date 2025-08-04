namespace Notification.API.Dtos
{
    public record NotificationResponseDto(Guid Id,
        string Message,
        bool IsRead,
        string? Type,
        DateTime CreatedAt,
        DateTime? ReadAt,
        Guid UserId);
}
