namespace Course.Domain.Models
{
    public class Quiz
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public int TotalMarks { get; set; }
        public int PassingMarks { get; set; }
        public int? TimeLimitInMinutes { get; set; } 

        public DateTime CreatedAt { get; set; }
        public Guid CreatedBy { get; set; } 

        public Guid LectureId { get; set; }
        public virtual Lecture Lecture { get; set; }

        public virtual ICollection<Question> Questions { get; set; } = [];
    }
}
