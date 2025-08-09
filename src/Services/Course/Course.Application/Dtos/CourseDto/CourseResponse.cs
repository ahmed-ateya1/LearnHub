using Course.Application.Dtos.SectionDto;

namespace Course.Application.Dtos.CourseDto
{
    public class CourseResponse
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string PosterUrl { get; set; }
        public string CourseLevel { get; set; }
        public string CourseStatus { get; set; }
        public string Language { get; set; }
        public int DurationInMinutes { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public Guid InstructorId { get; set; }
        public string InstructorName { get; set; }   

        public Guid CategoryId { get; set; }
        public string CategoryName { get; set; }

        public double AverageRating { get; set; }
        public int ReviewCount { get; set; }

        public ICollection<SectionResponse> Sections { get; set; } = new List<SectionResponse>();
    }
}
