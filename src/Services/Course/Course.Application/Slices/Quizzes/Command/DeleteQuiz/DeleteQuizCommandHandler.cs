using FluentValidation;

namespace Course.Application.Slices.Quizzes.Command.DeleteQuiz
{
    public record DeleteQuizCommand(Guid Id) : ICommand<bool>;

    public class DeleteQuizValidator : AbstractValidator<DeleteQuizCommand>
    {
        public DeleteQuizValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("quiz id is required");
        }
    }

    public class DeleteQuizCommandHandler(IQuizService quizService)
        : ICommandHandler<DeleteQuizCommand, bool>
    {
        public async Task<bool> Handle(DeleteQuizCommand request, CancellationToken cancellationToken)
        {
            return await quizService.DeleteQuizAsync(request.Id);
        }
    }
}