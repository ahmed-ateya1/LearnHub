using FluentValidation;

namespace Course.Application.Slices.Lectures.Command.DeleteLecture
{
    public record DeleteLectureCommand(Guid Id) : ICommand<bool>;

    public class DeleteLectureValidator : AbstractValidator<DeleteLectureCommand>
    {
        public DeleteLectureValidator()
        {
            RuleFor(c => c.Id)
                .NotEmpty()
                .WithMessage("Lecture Id is required");
        }
    }

    public class DeleteLectureCommandHandler(ILectureService lectureService)
        : ICommandHandler<DeleteLectureCommand, bool>
    {
        public async Task<bool> Handle(DeleteLectureCommand request, CancellationToken cancellationToken)
        {
            return await lectureService.DeleteLectureAsync(request.Id);
        }
    }
}
