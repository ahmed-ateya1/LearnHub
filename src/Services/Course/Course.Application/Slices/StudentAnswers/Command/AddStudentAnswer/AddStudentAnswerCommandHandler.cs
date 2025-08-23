using Course.Application.Dtos.StudentAnswerDto;
using FluentValidation;

namespace Course.Application.Slices.StudentAnswers.Command.AddStudentAnswer
{
    public record AddStudentAnswerCommand(StudentAnswerAddRequest StudentAnswerAddRequest) : ICommand<StudentAnswerResponse>;

    public class AddSystemAnswerCommandValidator : AbstractValidator<AddStudentAnswerCommand>
    {
        public AddSystemAnswerCommandValidator()
        {
            RuleFor(x => x.StudentAnswerAddRequest).NotNull().WithMessage("Student answer request cannot be null.");
            RuleFor(x => x.StudentAnswerAddRequest.StudentId).NotEmpty().WithMessage("Student ID is required.");
            RuleFor(x => x.StudentAnswerAddRequest.QuestionId).NotEmpty().WithMessage("Question ID is required.");
            When(x => x.StudentAnswerAddRequest.SelectedAnswerId == null, () =>
            {
                RuleFor(x => x.StudentAnswerAddRequest.AnswerText)
                    .NotEmpty()
                    .WithMessage("Either SelectedAnswerId or AnswerText must be provided.");
            });
        }
    }
    public class AddStudentAnswerCommandHandler(IStudentAnswerService studentAnswerService) 
        : ICommandHandler<AddStudentAnswerCommand, StudentAnswerResponse>
    {
        public async Task<StudentAnswerResponse> Handle(AddStudentAnswerCommand request, CancellationToken cancellationToken)
        {
            return await studentAnswerService.AddStudentAnswerAsync(request.StudentAnswerAddRequest);
        }
    }
}
