namespace Course.Domain.Models
{
    public class Lecture
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string VideoUrl { get; set; }

        public Guid SectionId { get; set; }
        public virtual Section Section { get; set; }

        public virtual ICollection<Quiz> Quizzes { get; set; } = [];
    }
}
