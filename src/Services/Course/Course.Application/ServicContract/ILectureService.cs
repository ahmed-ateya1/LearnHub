using Course.Application.Dtos.LectureDto;

namespace Course.Application.ServicContract
{
    public interface ILectureService
    {
        Task<LectureResponse> AddLectureAsync(LectureAddRequest request);
        Task<LectureResponse> UpdateLectureAsync(LectureUpdateRequest request);
        Task<bool> DeleteLectureAsync(Guid id);
        Task<LectureResponse> GetLectureByIdAsync(Expression<Func<Lecture, bool>> expression);
        Task<IEnumerable<LectureResponse>> GetLecturesByAsync(Expression<Func<Lecture,bool>>? expression = null);
    }
}
