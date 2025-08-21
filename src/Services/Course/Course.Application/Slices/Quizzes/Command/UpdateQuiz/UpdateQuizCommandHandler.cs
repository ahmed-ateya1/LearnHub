using Course.Application.Dtos.QuizDto;
using FluentValidation;

namespace Course.Application.Slices.Quizzes.Command.UpdateQuiz
{
    public record UpdateQuizCommand(QuizUpdateRequest QuizUpdate) : ICommand<QuizResponse>;

    public class UpdateQuizValidator : AbstractValidator<UpdateQuizCommand>
    {
        public UpdateQuizValidator()
        {
            RuleFor(x => x.QuizUpdate.Id)
                .NotEmpty().WithMessage("quiz id is required");

            RuleFor(x => x.QuizUpdate.Title)
                .NotNull().WithMessage("title can't be null")
                .MinimumLength(10).WithMessage("minimum length is 10")
                .MaximumLength(50).WithMessage("maximum length is 50");

            RuleFor(x => x.QuizUpdate.PassingMarks)
                .NotEmpty().WithMessage("Passing Mark is required");

            RuleFor(x => x.QuizUpdate.TotalMarks)
                .NotEmpty().WithMessage("Total Mark is required");

            RuleFor(x => x.QuizUpdate.CreatedBy)
                .NotEmpty().WithMessage("instrutor id is required");

            RuleFor(x => x.QuizUpdate.LectureId)
               .NotEmpty().WithMessage("lecture id is required");
        }
    }

    public class UpdateQuizCommandHandler(IQuizService quizService)
        : ICommandHandler<UpdateQuizCommand, QuizResponse>
    {
        public async Task<QuizResponse> Handle(UpdateQuizCommand request, CancellationToken cancellationToken)
        {
            return await quizService.UpdateQuizAsync(request.QuizUpdate);
        }
    }
}