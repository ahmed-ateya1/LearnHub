using BuildingBlocks.CQRS;

namespace Course.Application.Course.Queries.GetAllCourses
{
    public record GetAllCoursesQuery : IQuery<IEnumerable<CourseResponse>>;
    public class GetAllCoursesQueryHandler(ICourseService courseService) : IQueryHandler<GetAllCoursesQuery, IEnumerable<CourseResponse>>
    {
        public async Task<IEnumerable<CourseResponse>> Handle(GetAllCoursesQuery request, CancellationToken cancellationToken)
        {
            var response = await courseService.GetAllCoursesAsync();

            return response;
        }
    }
}
