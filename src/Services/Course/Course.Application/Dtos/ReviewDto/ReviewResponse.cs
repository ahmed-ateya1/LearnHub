namespace Course.Application.Dtos.ReviewDto
{
    public class ReviewResponse
    {
         public Guid Id { get; set; }
         public string Content { get; set; }
         public int Rating { get; set; }
         public DateTime CreatedAt { get; set; }
         public Guid CourseId { get; set; }
         public Guid UserId { get; set; }
         public Guid? ParentReviewId { get; set; }
    }
}
