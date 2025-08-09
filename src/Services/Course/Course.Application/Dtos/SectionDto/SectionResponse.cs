namespace Course.Application.Dtos.SectionDto
{
    public class SectionResponse
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public Guid CourseId { get; set; }
    }
}
