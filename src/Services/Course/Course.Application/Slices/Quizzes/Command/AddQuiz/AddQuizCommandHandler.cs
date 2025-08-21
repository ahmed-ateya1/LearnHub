using Course.Application.Dtos.QuizDto;
using FluentValidation;

namespace Course.Application.Slices.Quizzes.Command.AddQuiz
{
    public record AddQuizCommand(QuizAddRequest QuizAdd) : ICommand<QuizResponse>;

    public class AddQuizValidator : AbstractValidator<AddQuizCommand>
    {
        public AddQuizValidator()
        {
            RuleFor(x => x.QuizAdd.Title)
                .NotNull().WithMessage("title can't be null")
                .MinimumLength(10).WithMessage("minimum length is 10")
                .MaximumLength(50).WithMessage("maximum length is 50");

            RuleFor(x => x.QuizAdd.PassingMarks)
                .NotEmpty().WithMessage("Passing Mark is required");

            RuleFor(x => x.QuizAdd.TotalMarks)
                .NotEmpty().WithMessage("Total Mark is required");

            RuleFor(x => x.QuizAdd.CreatedBy)
                .NotEmpty().WithMessage("instrutor id is required");

            RuleFor(x => x.QuizAdd.LectureId)
               .NotEmpty().WithMessage("lecture id is required");
        }
    }
    public class AddQuizCommandHandler(IQuizService quizService) 
        : ICommandHandler<AddQuizCommand, QuizResponse>
    {
        public async Task<QuizResponse> Handle(AddQuizCommand request, CancellationToken cancellationToken)
        {
            return await quizService.AddQuizAsync(request.QuizAdd);
        }
    }
}
