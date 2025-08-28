namespace BuildingBlocks.Messaging.Events
{
   public record CoursePublishedEvent(
        Guid CourseId,
        string Title,
        string Description,
        decimal Price,
        Guid InstructorId,
        Guid CategoryId,
        DateTime PublishedDate);
}
