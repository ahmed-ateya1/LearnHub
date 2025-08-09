using Course.Domain.Enums;

namespace Course.Domain.Models
{
    public class Course
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string PosterUrl { get; set; }
        public Guid InstructorId { get; set; }

        public CourseLevel CourseLevel { get; set; }
        public CourseStatus CourseStatus { get; set; }
        public string Language { get; set; }

        public int DurationInMinutes { get; set; } 
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public Guid CategoryId { get; set; }
        public virtual Category Category { get; set; }

        public virtual ICollection<Section> Sections { get; set; } = [];
        public virtual ICollection<Review> Reviews { get; set; } = [];
    }
}
