using Course.Application.Dtos.StudentAnswerDto;

namespace Course.Application.Slices.StudentAnswers.Queries.GetStudentAnswerForQuiz
{
    public record GetStudentAnswerForQuizQuery(Guid StudentId, Guid QuizId) : IQuery<IEnumerable<StudentAnswerResponse>>;
    public class GetStudentAnswerForQuizQueryHandler(IStudentAnswerService studentAnswerService)
        : IQueryHandler<GetStudentAnswerForQuizQuery, IEnumerable<StudentAnswerResponse>>
    {
        public async Task<IEnumerable<StudentAnswerResponse>> Handle(GetStudentAnswerForQuizQuery request, CancellationToken cancellationToken)
        {
           return await studentAnswerService.GetAllStudentAnswersAsync(sa => sa.StudentId == request.StudentId && sa.Question.QuizId == request.QuizId);
        }
    }
}
