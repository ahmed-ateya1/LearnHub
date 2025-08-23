using Course.Application.Dtos.QuestionDto;
using FluentValidation;

namespace Course.Application.Slices.Questions.Command.AddQuestion
{
    public record AddQuestionCommand(QuestionAddRequest Request) : ICommand<QuestionResponse>;

    public class AddQuestionCommandValidator : AbstractValidator<AddQuestionCommand>
    {
        public AddQuestionCommandValidator()
        {
            RuleFor(x=>x.Request).NotNull().WithMessage("Request cannot be null");

            RuleFor(x => x.Request.QuestionText)
                .NotEmpty().WithMessage("Question text is required")
                .MaximumLength(1000).WithMessage("Question text cannot exceed 1000 characters");

            RuleFor(x => x.Request.QuestionType)
                .NotEmpty().WithMessage("Question type is required")
                .IsInEnum().WithMessage("Invalid question type. Allowed types are: MultipleChoice, TrueFalse, ShortAnswer");
            RuleFor(x => x.Request.Marks)
                .GreaterThan(0).WithMessage("Marks must be greater than zero");

            RuleFor(x => x.Request.Order)
                .GreaterThan(0).WithMessage("Order must be greater than zero");

            RuleFor(x => x.Request.QuizId)
                .NotEmpty().WithMessage("QuizId is required");

            RuleFor(x => x.Request.Answers)
                .NotEmpty().WithMessage("At least one answer is required")
                .Must(answers => answers != null && answers.Count > 0).WithMessage("Answers cannot be null or empty");


        }
    }
    public class AddQuestionCommandHandler(IQuestionService questionService)
        : ICommandHandler<AddQuestionCommand, QuestionResponse>
    {

        public async Task<QuestionResponse> Handle(AddQuestionCommand request, CancellationToken cancellationToken)
        {
            return await questionService.AddQuestionAsync(request.Request);
        }
    }
}
