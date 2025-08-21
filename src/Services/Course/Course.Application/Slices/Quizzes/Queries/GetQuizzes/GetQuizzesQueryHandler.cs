using Course.Application.Dtos.QuizDto;

namespace Course.Application.Slices.Quizzes.Queries.GetQuizzes
{
    public record GetQuizzesQuery : IQuery<IEnumerable<QuizResponse>>;
    public class GetQuizzesQueryHandler(IQuizService quizService)
        : IQueryHandler<GetQuizzesQuery, IEnumerable<QuizResponse>>
    {
        public async Task<IEnumerable<QuizResponse>> Handle(GetQuizzesQuery request, CancellationToken cancellationToken)
        {
            return await quizService.GetQuizzesByAsync();
        }
    }
}
