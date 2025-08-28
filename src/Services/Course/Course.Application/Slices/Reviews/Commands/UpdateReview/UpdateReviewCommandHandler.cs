using Course.Application.Dtos.ReviewDto;
using FluentValidation;

namespace Course.Application.Slices.Reviews.Commands.UpdateReview
{
    public record UpdateReviewCommand(ReviewUpdateRequest ReviewUpdateRequest) : ICommand<ReviewResponse>;

    public class UpdateReviewCommandValidator : AbstractValidator<UpdateReviewCommand>
    {
        public UpdateReviewCommandValidator()
        {
            RuleFor(x => x.ReviewUpdateRequest).NotNull().WithMessage("ReviewUpdateRequest cannot be null.");
            RuleFor(x => x.ReviewUpdateRequest.Id)
                .NotEmpty().WithMessage("Review Id cannot be empty.");
            RuleFor(x => x.ReviewUpdateRequest.Content)
                .NotEmpty().WithMessage("Content cannot be empty.")
                .MaximumLength(1000).WithMessage("Content cannot exceed 1000 characters.");
            RuleFor(x => x.ReviewUpdateRequest.Rating)
                .InclusiveBetween(1, 5).WithMessage("Rating must be between 1 and 5.");
        }
    }
    public class UpdateReviewCommandHandler(IReviewService reviewService)
        : ICommandHandler<UpdateReviewCommand, ReviewResponse>
    {
        public async Task<ReviewResponse> Handle(UpdateReviewCommand request, CancellationToken cancellationToken)
        {
            return await reviewService.UpdateReviewAsync(request.ReviewUpdateRequest);
        }
    }
}
