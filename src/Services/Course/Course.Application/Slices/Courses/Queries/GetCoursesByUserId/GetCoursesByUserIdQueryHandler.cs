namespace Course.Application.Slices.Courses.Queries.GetCoursesByUserId
{
    public record GetCoursesByUserIdQuery(Guid UserId) : IQuery<IEnumerable<CourseResponse>>;
    internal class GetCoursesByUserIdQueryHandler(ICourseService courseService)
        : IQueryHandler<GetCoursesByUserIdQuery, IEnumerable<CourseResponse>>
    {
        public async Task<IEnumerable<CourseResponse>> Handle(GetCoursesByUserIdQuery request, CancellationToken cancellationToken)
        {
            return await courseService.GetAllCoursesAsync(x => x.InstructorId == request.UserId);
        }
    }
}
