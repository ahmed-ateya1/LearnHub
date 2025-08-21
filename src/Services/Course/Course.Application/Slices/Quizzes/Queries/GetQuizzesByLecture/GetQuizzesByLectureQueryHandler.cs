using Course.Application.Dtos.QuizDto;

namespace Course.Application.Slices.Quizzes.Queries.GetQuizzesByLecture
{
    public record class GetQuizzesByLectureQuery(
        Guid LectureId
    ) : IQuery<IEnumerable<QuizResponse>>;
    public class GetQuizzesByLectureQueryHandler(IQuizService quizService)
        : IQueryHandler<GetQuizzesByLectureQuery, IEnumerable<QuizResponse>>
    {
        public async Task<IEnumerable<QuizResponse>> Handle(GetQuizzesByLectureQuery request, CancellationToken cancellationToken)
        {
            return await quizService.GetQuizzesByAsync(x=>x.LectureId == request.LectureId);
        }
    }
}
