using Course.Application.Dtos.QuizDto;

namespace Course.Application.Slices.Quizzes.Queries.GetQuizById
{
    public record GetQuizByIdQuery(Guid Id) : IQuery<QuizResponse>;
    public class GetQuizByIdQueryHandler(IQuizService quizService)
        : IQueryHandler<GetQuizByIdQuery, QuizResponse>
    {
        public async Task<QuizResponse> Handle(GetQuizByIdQuery request, CancellationToken cancellationToken)
        {
            return await quizService.GetQuizByIdAsync(request.Id);
        }
    }
}
