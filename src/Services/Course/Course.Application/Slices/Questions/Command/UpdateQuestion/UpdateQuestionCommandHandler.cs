using Course.Application.Dtos.QuestionDto;
using FluentValidation;

namespace Course.Application.Slices.Questions.Command.UpdateQuestion

{
    public record UpdateQuestionCommand(QuestionUpdateRequest Request) : ICommand<QuestionResponse>;

    public class UpdateQuestionCommandValidator : AbstractValidator<UpdateQuestionCommand>
    {
        public UpdateQuestionCommandValidator()
        {
            RuleFor(x => x.Request).NotNull().WithMessage("Request cannot be null");
            RuleFor(x => x.Request.QuestionText)
                .NotEmpty().WithMessage("Question text is required")
                .MaximumLength(1000).WithMessage("Question text cannot exceed 1000 characters");

            RuleFor(x => x.Request.Marks)
                .GreaterThan(0).WithMessage("Marks must be greater than zero");
            RuleFor(x => x.Request.Order)
                .GreaterThan(0).WithMessage("Order must be greater than zero");

            RuleFor(x => x.Request.Answers)
                .NotEmpty().WithMessage("At least one answer is required")
                .Must(answers => answers != null && answers.Count > 0).WithMessage("Answers cannot be null or empty");
        }
    }
    public class UpdateQuestionCommandHandler(IQuestionService questionService)
        : ICommandHandler<UpdateQuestionCommand, QuestionResponse>
    {
        public async Task<QuestionResponse> Handle(UpdateQuestionCommand request, CancellationToken cancellationToken)
        {
            return await questionService.UpdateQuestionAsync(request.Request);
        }
    }
}
