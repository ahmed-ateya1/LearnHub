namespace Course.Application.Course.Queries.GetCOurseByName

{
    public record GetCourseByNameQuery(string CourseName) : IQuery<IEnumerable<CourseResponse>>;
    internal class GetCourseByNameQueryHandler (ICourseService courseService)
        : IQueryHandler<GetCourseByNameQuery, IEnumerable<CourseResponse>>
    {
        public async Task<IEnumerable<CourseResponse>> Handle(GetCourseByNameQuery request, CancellationToken cancellationToken)
        {
            return await courseService.GetAllCoursesAsync(x=>x.Title.ToUpper().Contains(request.CourseName.ToUpper()));
        }
    }
}
