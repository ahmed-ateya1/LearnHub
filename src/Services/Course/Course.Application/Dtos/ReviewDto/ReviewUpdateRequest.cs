namespace Course.Application.Dtos.ReviewDto
{
    public class ReviewUpdateRequest
    {
         public Guid Id { get; set; }
         public string Content { get; set; }
         public int Rating { get; set; }
    }
}
