using Course.Application.Dtos.QuestionDto;

namespace Course.Application.ServicContract
{
    public interface IQuestionService
    {
        Task<QuestionResponse> AddQuestionAsync(QuestionAddRequest request);
        Task<QuestionResponse> UpdateQuestionAsync(QuestionUpdateRequest request);
        Task<bool> DeleteQuestionAsync(Guid id);
        Task<QuestionResponse> GetQuestionByIdAsync(Guid id);
        Task<IEnumerable<QuestionResponse>> GetAllQuestionsAsync(Expression<Func<Question,bool>>?filter = null);
    }
}
