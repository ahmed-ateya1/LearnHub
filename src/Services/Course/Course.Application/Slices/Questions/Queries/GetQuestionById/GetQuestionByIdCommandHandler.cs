using Course.Application.Dtos.QuestionDto;

namespace Course.Application.Slices.Questions.Queries.GetQuestionById
{
    public record GetQuestionByIdCommand(Guid Id) : IQuery<QuestionResponse>;
    public class GetQuestionByIdCommandHandler(IQuestionService questionService)
        : IQueryHandler<GetQuestionByIdCommand, QuestionResponse>
    {
        public async Task<QuestionResponse> Handle(GetQuestionByIdCommand request, CancellationToken cancellationToken)
        {
            return await questionService.GetQuestionByIdAsync(request.Id);
        }
    }
}
