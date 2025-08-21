using Course.Application.Dtos.QuizDto;

namespace Course.Application.ServicContract
{
    public interface IQuizService
    {
        Task<QuizResponse> AddQuizAsync(QuizAddRequest request);
        Task<QuizResponse> UpdateQuizAsync(QuizUpdateRequest request);
        Task<bool> DeleteQuizAsync(Guid id);
        Task<QuizResponse> GetQuizByIdAsync(Guid id);
        Task<IEnumerable<QuizResponse>> GetQuizzesByAsync(Expression<Func<Quiz,bool>>? filter=null);
    }
}
