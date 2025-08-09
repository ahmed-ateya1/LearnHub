namespace Course.Domain.Models
{
    public class Review
    {
        public Guid Id { get; set; }
        public string Content { get; set; }
        public int Rating { get; set; } 
        public DateTime CreatedAt { get; set; }

        public Guid CourseId { get; set; }
        public virtual Course Course { get; set; }

        public Guid UserId { get; set; }

        public Guid? ParentReviewId { get; set; }
        public virtual Review? ParentReview { get; set; }

        public virtual ICollection<Review> Replies { get; set; } = [];
    }
}
