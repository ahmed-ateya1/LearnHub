namespace Course.Application.Slices.Questions.Command.DeleteQuestion
{
    public record DeleteQuestionCommand(Guid Id) : ICommand<bool>;
    public class DeleteQuestionCommandHandler (IQuestionService questionService)
        : ICommandHandler<DeleteQuestionCommand, bool>
    {
        public async Task<bool> Handle(DeleteQuestionCommand request, CancellationToken cancellationToken)
        {
            return await questionService.DeleteQuestionAsync(request.Id);
        }
    }
}
