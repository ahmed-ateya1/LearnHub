using Course.Domain.Enums;

namespace Course.Application.ServicContract
{
    public interface ICourseService
    {
        Task<CourseResponse> CreateCourseAsync(CourseAddRequest? courseAddRequest);
        Task<CourseResponse> UpdateCourseAsync(CourseUpdateRequest? courseUpdateRequest);
        Task<bool> UpdateCourseStatusAsync(Guid id, CourseStatus courseStatus);
        Task<bool> DeleteCourseAsync(Guid id);
        Task<CourseResponse> GetCourseByAsync(Expression<Func<Domain.Models.Course, bool>>filter);
        Task<IEnumerable<CourseResponse>> GetAllCoursesAsync(Expression<Func<Domain.Models.Course, bool>>? filter = null);
    }
}
