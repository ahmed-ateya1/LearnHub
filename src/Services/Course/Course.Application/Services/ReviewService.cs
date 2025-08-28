using Course.Application.Dtos.ReviewDto;

namespace Course.Application.Services
{
    public class ReviewService(IUnitOfWork unitOfWork , ILogger<ReviewService> logger)
        : IReviewService
    {
        private async Task ExecuteWithTransactionAsync(Func<Task> action)
        {
            using var transaction = await unitOfWork.BeginTransactionAsync();
            try
            {
                await action();
                await unitOfWork.CommitTransactionAsync();
            }
            catch (Exception ex)
            {
                await unitOfWork.RollbackTransactionAsync();
                logger.LogError(ex, "An error occurred while executing the transaction.");
                throw;
            }
        }
        public async Task<ReviewResponse> AddReviewAsync(ReviewAddRequest request)
        {
            if(request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }
            var course = await unitOfWork.Repository<Domain.Models.Course>()
                .GetByAsync(x=>x.Id == request.CourseId);
            if(course == null)
            {
                logger.LogWarning("Course with Id {CourseId} not found.", request.CourseId);
                throw new CourseNotFoundException($"Course with Id {request.CourseId} not found.");
            }
            var review = request.Adapt<Review>();

            await ExecuteWithTransactionAsync(async () =>
            {
                await unitOfWork.Repository<Review>().CreateAsync(review);
            });

            return review.Adapt<ReviewResponse>();
        }

        public async Task<bool> DeleteReviewAsync(Guid reviewId)
        {
            var review =await unitOfWork.Repository<Review>().GetByAsync(x => x.Id == reviewId); 

            if (review == null)
            {
                logger.LogWarning("Review with Id {ReviewId} not found.", reviewId);
                throw new ReviewNotFoundException($"Review with Id {reviewId} not found.");
            }

            await ExecuteWithTransactionAsync(async () =>
            {
                await unitOfWork.Repository<Review>().DeleteAsync(review);
            });
            return true;
        }

        public async Task<ReviewResponse> GetReviewByIdAsync(Guid reviewId)
        {
            var review = await unitOfWork.Repository<Review>().GetByAsync(x => x.Id == reviewId);
            if (review == null)
            {
                logger.LogWarning("Review with Id {ReviewId} not found.", reviewId);
                throw new ReviewNotFoundException($"Review with Id {reviewId} not found.");
            }

            return review.Adapt<ReviewResponse>();
        }

        public async Task<IEnumerable<ReviewResponse>> GetReviewsByAsync(Expression<Func<Review, bool>>? filter = null)
        {
            var reviews = await unitOfWork.Repository<Review>().GetAllAsync(filter);  

            if (reviews == null || !reviews.Any())
            {
                logger.LogInformation("No reviews found matching the specified criteria.");
                return Enumerable.Empty<ReviewResponse>();
            }
            return reviews.Adapt<IEnumerable<ReviewResponse>>();
        }

        public async Task<ReviewResponse> UpdateReviewAsync(ReviewUpdateRequest request)
        {
            if(request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            var existingReview = await unitOfWork.Repository<Review>().GetByAsync(x => x.Id == request.Id);
            if (existingReview == null)
            {
                logger.LogWarning("Review with Id {ReviewId} not found.", request.Id);
                throw new ReviewNotFoundException($"Review with Id {request.Id} not found.");
            }

            request.Adapt(existingReview);

            await ExecuteWithTransactionAsync(async () =>
            {
                await unitOfWork.Repository<Review>().UpdateAsync(existingReview);
            });

            return existingReview.Adapt<ReviewResponse>();
        }
    }
}
