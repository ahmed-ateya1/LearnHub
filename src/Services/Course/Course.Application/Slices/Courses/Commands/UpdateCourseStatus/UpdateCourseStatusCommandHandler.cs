using Course.Domain.Enums;

namespace Course.Application.Slices.Courses.Commands.UpdateCourseStatus
{
    public record UpdateCourseStatusCommand(Guid CourseId, CourseStatus Status) : ICommand<bool>;
    public class UpdateCourseStatusCommandHandler(ICourseService courseService)
        : ICommandHandler<UpdateCourseStatusCommand, bool>
    {
        public async Task<bool> Handle(UpdateCourseStatusCommand request, CancellationToken cancellationToken)
        {
            return await courseService.UpdateCourseStatusAsync(request.CourseId, request.Status);
        }
    }
}
