using Course.Application.Dtos.StudentAnswerDto;

namespace Course.Application.ServicContract
{
    public interface IStudentAnswerService
    {
        Task<StudentAnswerResponse> AddStudentAnswerAsync(StudentAnswerAddRequest request);
        Task<StudentAnswerResponse> UpdateStudentAnswerAsync(StudentAnswerUpdateRequest request);
        Task<StudentAnswerResponse> GetStudentAnswerByIdAsync(Guid id);
        Task<IEnumerable<StudentAnswerResponse>> GetAllStudentAnswersAsync(Expression<Func<StudentAnswer,bool>>? filter = null);
        Task<bool> DeleteStudentAnswerAsync(Guid id);
    }
}
