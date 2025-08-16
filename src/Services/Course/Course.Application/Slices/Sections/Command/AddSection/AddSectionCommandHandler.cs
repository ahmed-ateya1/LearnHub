using Course.Application.Dtos.SectionDto;
using FluentValidation;

namespace Course.Application.Slices.Sections.Command.AddSection
{
    public record AddSectionCommand(SectionAddRequest Request): ICommand<SectionResponse>;

    public class AddSectionValidator : AbstractValidator<AddSectionCommand>
    {
        public AddSectionValidator()
        {
            RuleFor(x => x.Request)
                .NotNull()
                .WithMessage("Section add request cannot be null.");
            RuleFor(x => x.Request.Title)
                .NotEmpty()
                .WithMessage("Section title cannot be empty.");
            RuleFor(x => x.Request.CourseId)
                .NotEmpty()
                .WithMessage("Course ID cannot be empty.");
        }
    }
    public class AddSectionCommandHandler(ISectionService sectionService)
        : ICommandHandler<AddSectionCommand, SectionResponse>
    {
        public async Task<SectionResponse> Handle(AddSectionCommand request, CancellationToken cancellationToken)
        {
            if (request == null || request.Request == null)
            {
                throw new ArgumentNullException(nameof(request), "Request cannot be null.");
            }
            return await sectionService.AddSectionAsync(request.Request);
        }
    }
}
