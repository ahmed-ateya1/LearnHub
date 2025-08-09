namespace Course.Domain.Models
{
    public class Section
    {
        public Guid Id { get; set; }
        public string Title { get; set; }

        public Guid CourseId { get; set; }
        public virtual Course Course { get; set; }

        public virtual ICollection<Lecture> Lectures { get; set; } = [];
    }
}
