using Course.Application.Dtos.ReviewDto;

namespace Course.Application.Slices.Reviews.Queries.GetReviewsByUser
{
    public record GetReviewsByUserQuery(Guid UserId) : IQuery<IEnumerable<ReviewResponse>>;
    public class GetReviewByUserQueryHandler (IReviewService reviewService)
        : IQueryHandler<GetReviewsByUserQuery, IEnumerable<ReviewResponse>>
    {
        public async Task<IEnumerable<ReviewResponse>> Handle(GetReviewsByUserQuery request, CancellationToken cancellationToken)
        {
            return await reviewService.GetReviewsByAsync(x => x.UserId == request.UserId);
        }
    }
}
