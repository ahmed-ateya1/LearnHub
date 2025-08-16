namespace Course.Application.Slices.Courses.Commands.DeleteCourse
{
    public record DeleteCourseCommand(Guid CourseId) : ICommand<bool>;
    public class DeleteCourseCommandHandler(ICourseService courseService)
        : ICommandHandler<DeleteCourseCommand, bool>
    {
        public async Task<bool> Handle(DeleteCourseCommand request, CancellationToken cancellationToken)
        {
           return await courseService.DeleteCourseAsync(request.CourseId);
        }
    }
}
