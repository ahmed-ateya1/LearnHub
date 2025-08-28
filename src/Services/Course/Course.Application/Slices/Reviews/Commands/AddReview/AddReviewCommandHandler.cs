using Course.Application.Dtos.ReviewDto;
using FluentValidation;

namespace Course.Application.Slices.Reviews.Commands.AddReview
{
    public record AddReviewCommand(ReviewAddRequest ReviewAddRequest) : ICommand<ReviewResponse>;

    public class AddReviewCommandValidator : AbstractValidator<AddReviewCommand>
    {
        public AddReviewCommandValidator()
        {
            RuleFor(x => x.ReviewAddRequest).NotNull().WithMessage("ReviewAddRequest cannot be null.");
            RuleFor(x => x.ReviewAddRequest.Content)
                .NotEmpty().WithMessage("Content cannot be empty.")
                .MaximumLength(1000).WithMessage("Content cannot exceed 1000 characters.");
            RuleFor(x => x.ReviewAddRequest.Rating)
                .InclusiveBetween(1, 5).WithMessage("Rating must be between 1 and 5.");
            RuleFor(x => x.ReviewAddRequest.CourseId)
                .NotEmpty().WithMessage("CourseId cannot be empty.");
            RuleFor(x => x.ReviewAddRequest.UserId)
                .NotEmpty().WithMessage("UserId cannot be empty.");
        }
    }
    public class AddReviewCommandHandler(IReviewService reviewService)
        : ICommandHandler<AddReviewCommand, ReviewResponse>
    {
        public async Task<ReviewResponse> Handle(AddReviewCommand request, CancellationToken cancellationToken)
        {
            return await reviewService.AddReviewAsync(request.ReviewAddRequest);
        }
    }
}
