using BuildingBlocks.CQRS;
using FluentValidation;

namespace Course.Application.Course.Commands.CreateCourse
{
    public record CreateCourseCommand(CourseAddRequest course):ICommand<CourseResponse>;

    public class CreateCourseValidator : AbstractValidator<CreateCourseCommand>
    {
        public CreateCourseValidator()
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

            RuleFor(c => c.course.InstructorId)
                .NotEmpty().WithMessage("InstructorId is required.");

            RuleFor(c => c.course.Language)
                .NotEmpty().WithMessage("Language is required.")
                .MaximumLength(50).WithMessage("Language must not exceed 50 characters.");

            RuleFor(c => c.course.CourseLevel)
                .IsInEnum().WithMessage("Invalid Course Level. Allowed values are Beginner, Intermediate, Advanced.");

            RuleFor(c => c.course.Poster)
                .NotNull().WithMessage("Poster is required.")
                .Must(file => file.Length > 0).WithMessage("Poster file must not be empty.")
                .Must(file => file.Length <= 5 * 1024 * 1024).WithMessage("Poster file size must not exceed 5MB.")
                .Must(file => file.ContentType == "image/jpeg" || file.ContentType == "image/png")
                .WithMessage("Poster must be a JPEG or PNG image.");
        }
    }
    public class CreateCourseCommandHandler(ICourseService courseService) 
        : ICommandHandler<CreateCourseCommand, CourseResponse>
    {
        public async Task<CourseResponse> Handle(CreateCourseCommand request, CancellationToken cancellationToken)
        {
            var response = await courseService.CreateCourseAsync(request.course);

            return response;
        }
    }
}
