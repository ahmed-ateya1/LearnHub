using Course.Application.Dtos.ReviewDto;

namespace Course.Application.ServicContract
{
    public interface IReviewService
    {
        Task<ReviewResponse> AddReviewAsync(ReviewAddRequest request);
        Task<IEnumerable<ReviewResponse>> GetReviewsByAsync(Expression<Func<Review,bool>>? filter = null);
        Task<ReviewResponse> GetReviewByIdAsync(Guid reviewId);
        Task<bool> DeleteReviewAsync(Guid reviewId);
        Task<ReviewResponse> UpdateReviewAsync(ReviewUpdateRequest request);
    }
}
