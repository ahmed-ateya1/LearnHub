namespace Course.Application.Slices.StudentAnswers.Command.DeleteStudentAnswer
{

    public record DeleteStudentAnswerCommand(Guid StudentAnswerId) : ICommand<bool>;
    public class DeleteStudentAnswerCommandHandler (IStudentAnswerService studentAnswerService)
        : ICommandHandler<DeleteStudentAnswerCommand, bool>
    {
        public async Task<bool> Handle(DeleteStudentAnswerCommand request, CancellationToken cancellationToken)
        {
            return await studentAnswerService.DeleteStudentAnswerAsync(request.StudentAnswerId);
        }
    }
}
