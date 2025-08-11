
namespace Course.Application.Course.Queries.GetCourseById
{
    public record GetCourseByIdQuery(Guid CourseId) : IQuery<CourseResponse>;
    public class GetCourseByIdQueryHandler(ICourseService courseService)
        : IQueryHandler<GetCourseByIdQuery, CourseResponse>
    {
        public async Task<CourseResponse> Handle(GetCourseByIdQuery request, CancellationToken cancellationToken)
        {
            return await courseService.GetCourseByAsync(c => c.Id == request.CourseId);
        }
    }
}
