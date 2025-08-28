using Course.Application.Dtos.ReviewDto;

namespace Course.Application.Slices.Reviews.Queries.GetReviewsByCourse
{
    public record GetReviewsByCourseQuery(Guid CourseId) : IQuery<IEnumerable<ReviewResponse>>;
    public class GetReviewByCourseQueryHandler (IReviewService reviewService)
        : IQueryHandler<GetReviewsByCourseQuery, IEnumerable<ReviewResponse>>
    {
        public async Task<IEnumerable<ReviewResponse>> Handle(GetReviewsByCourseQuery request, CancellationToken cancellationToken)
        {
            return await reviewService.GetReviewsByAsync(x=>x.CourseId == request.CourseId);
        }
    }
}
