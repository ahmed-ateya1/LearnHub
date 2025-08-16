using FluentValidation;

namespace Course.Application.Slices.Courses.Commands.UpdateCourse
{
    public record UpdateCourseCommand(CourseUpdateRequest course) : ICommand<CourseResponse>;

    public class CourseUpdateValidator : AbstractValidator<UpdateCourseCommand>
    {
        public CourseUpdateValidator()
        {
            RuleFor(c => c.course.Title)
                 .NotEmpty().WithMessage("Title is required.")
                 .MaximumLength(100).WithMessage("Title must not exceed 100 characters.");
            RuleFor(c => c.course.Description)
                .NotEmpty().WithMessage("Description is required.")
                .MaximumLength(500).WithMessage("Description must not exceed 500 characters.");
            RuleFor(c => c.course.CategoryId)
                .NotEmpty().WithMessage("CategoryId is required");

            RuleFor(c => c.course.Price)
                .GreaterThanOrEqualTo(0).WithMessage("Price must be a positive number.");


            RuleFor(c => c.course.Language)
                .NotEmpty().WithMessage("Language is required.")
                .MaximumLength(50).WithMessage("Language must not exceed 50 characters.");

            RuleFor(c => c.course.CourseLevel)
                .IsInEnum().WithMessage("Invalid Course Level. Allowed values are Beginner, Intermediate, Advanced.");

            RuleFor(c => c.course.Poster)
                .Must(file => file.Length <= 5 * 1024 * 1024).WithMessage("Poster file size must not exceed 5MB.")
                .Must(file => file.ContentType == "image/jpeg" || file.ContentType == "image/png")
                .WithMessage("Poster must be a JPEG or PNG image.");
        }
    }
    public class UpdateCourseCommandHandler(ICourseService courseService)
        : ICommandHandler<UpdateCourseCommand, CourseResponse>
    {
        public async Task<CourseResponse> Handle(UpdateCourseCommand command, CancellationToken cancellationToken)
        {
            return await courseService.UpdateCourseAsync(command.course);
        }
    }
}
