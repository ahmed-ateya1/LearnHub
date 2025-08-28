namespace Course.Application.Dtos.ReviewDto
{
    public class ReviewAddRequest
    {
         public string Content { get; set; }
         public int Rating { get; set; }
         public Guid CourseId { get; set; }
         public Guid UserId { get; set; }
         public Guid? ParentReviewId { get; set; }
    }
}
