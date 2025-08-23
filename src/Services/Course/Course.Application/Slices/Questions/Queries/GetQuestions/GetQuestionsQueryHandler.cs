using Course.Application.Dtos.QuestionDto;

namespace Course.Application.Slices.Questions.Queries.GetQuestions
{
    public record GetQuestionsQuery(Guid QuizId) : IQuery<IEnumerable<QuestionResponse>>;
    public class GetQuestionsQueryHandler(IQuestionService questionService)
        : IQueryHandler<GetQuestionsQuery, IEnumerable<QuestionResponse>>
    {
        public async Task<IEnumerable<QuestionResponse>> Handle(GetQuestionsQuery request, CancellationToken cancellationToken)
        {
            return await questionService.GetAllQuestionsAsync(q => q.QuizId == request.QuizId);
        }
    }
}
