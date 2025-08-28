using Course.Application.Dtos.ReviewDto;

namespace Course.Application.Slices.Reviews.Queries.GetReviewById
{
    public record GetReviewByIdQuery(Guid ReviewId) : IQuery<ReviewResponse>;
    public class GetReviewByIdQueryHandler(IReviewService reviewService)
        : IQueryHandler<GetReviewByIdQuery, ReviewResponse>
    {
        public async Task<ReviewResponse> Handle(GetReviewByIdQuery request, CancellationToken cancellationToken)
        {
            return await reviewService.GetReviewByIdAsync(request.ReviewId);
        }
    }
}
