namespace Course.Application.Course.Queries.GetCoursesByCategoryId
{
    public record GetCoursesByCategoryQuery(Guid CategoryId) : IQuery<IEnumerable<CourseResponse>>;
    internal class GetCoursesByCategoryQueryHandler (ICourseService courseService)
        : IQueryHandler<GetCoursesByCategoryQuery, IEnumerable<CourseResponse>>
    {
        public async Task<IEnumerable<CourseResponse>> Handle(GetCoursesByCategoryQuery request, CancellationToken cancellationToken)
        {
            return await courseService.GetAllCoursesAsync(x => x.CategoryId == request.CategoryId);
        }
    }
}
