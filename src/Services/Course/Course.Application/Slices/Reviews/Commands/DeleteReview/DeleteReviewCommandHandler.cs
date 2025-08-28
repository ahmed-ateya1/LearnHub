namespace Course.Application.Slices.Reviews.Commands.DeleteReview
{
    public record DeleteReviewCommand(Guid ReviewId) : ICommand<bool>;
    public class DeleteReviewCommandHandler (IReviewService reviewService)
        : ICommandHandler<DeleteReviewCommand, bool>
    {
        public async Task<bool> Handle(DeleteReviewCommand request, CancellationToken cancellationToken)
        {
            return await reviewService.DeleteReviewAsync(request.ReviewId);
        }
    }
}
