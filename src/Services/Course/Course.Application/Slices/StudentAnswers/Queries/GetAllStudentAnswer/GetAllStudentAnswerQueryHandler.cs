using Course.Application.Dtos.StudentAnswerDto;

namespace Course.Application.Slices.StudentAnswers.Queries.GetAllStudentAnswer
{
    public record GetAllStudentAnswerQuery(Guid quizId) : IQuery<IEnumerable<StudentAnswerResponse>>;
    public class GetAllStudentAnswerQueryHandler (IStudentAnswerService studentAnswerService)
        : IQueryHandler<GetAllStudentAnswerQuery, IEnumerable<StudentAnswerResponse>>
    {
        public async Task<IEnumerable<StudentAnswerResponse>> Handle(GetAllStudentAnswerQuery request, CancellationToken cancellationToken)
        {
           return await studentAnswerService.GetAllStudentAnswersAsync(x=>x.Question.QuizId == request.quizId);
        }
    }
}
