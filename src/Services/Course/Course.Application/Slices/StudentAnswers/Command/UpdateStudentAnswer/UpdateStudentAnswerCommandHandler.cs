using Course.Application.Dtos.StudentAnswerDto;
using FluentValidation;

namespace Course.Application.Slices.StudentAnswers.Command.UpdateStudentAnswer
{
    public record UpdateStudentAnswerCommand(StudentAnswerUpdateRequest StudentAnswerUpdateRequest) : ICommand<StudentAnswerResponse>;

    public class UpdateStudentAnswerCommandValidator : AbstractValidator<UpdateStudentAnswerCommand>
    {
        public UpdateStudentAnswerCommandValidator()
        {
            RuleFor(x => x.StudentAnswerUpdateRequest).NotNull().WithMessage("Student answer update request cannot be null.");
            RuleFor(x => x.StudentAnswerUpdateRequest.Id).NotEmpty().WithMessage("Student answer ID is required.");
            When(x => x.StudentAnswerUpdateRequest.SelectedAnswerId == null, () =>
            {
                RuleFor(x => x.StudentAnswerUpdateRequest.AnswerText)
                    .NotEmpty()
                    .WithMessage("Either SelectedAnswerId or AnswerText must be provided.");
            });
        }
    }
    internal class UpdateStudentAnswerCommandHandler(IStudentAnswerService studentAnswerService)
        : ICommandHandler<UpdateStudentAnswerCommand, StudentAnswerResponse>
    {
        public async Task<StudentAnswerResponse> Handle(UpdateStudentAnswerCommand request, CancellationToken cancellationToken)
        {
            return await studentAnswerService.UpdateStudentAnswerAsync(request.StudentAnswerUpdateRequest);
        }
    }
}
