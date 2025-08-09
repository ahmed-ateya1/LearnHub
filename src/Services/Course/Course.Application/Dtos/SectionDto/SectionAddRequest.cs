namespace Course.Application.Dtos.SectionDto
{
    public class SectionAddRequest
    {
        public string Title { get; set; }
        public Guid CourseId { get; set; }
    }
}
