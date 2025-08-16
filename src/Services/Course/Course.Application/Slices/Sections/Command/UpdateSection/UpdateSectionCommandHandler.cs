using Course.Application.Dtos.SectionDto;
using FluentValidation;

namespace Course.Application.Slices.Sections.Command.UpdateSection
{
    public record UpdateSectionCommand(SectionUpdateRequest Request) : ICommand<SectionResponse>;

    public class UpdateSectionValidator : AbstractValidator<UpdateSectionCommand>
    {
        public UpdateSectionValidator()
        {
            RuleFor(x => x.Request).NotNull().WithMessage("Section update request cannot be null.");
            RuleFor(x => x.Request.Id).NotEmpty().WithMessage("Section ID is required.");
            RuleFor(x => x.Request.Title).NotEmpty().WithMessage("Section title is required.");
        }
    }
    public class UpdateSectionCommandHandler(ISectionService sectionService)
        : ICommandHandler<UpdateSectionCommand, SectionResponse>
    {
        public async Task<SectionResponse> Handle(UpdateSectionCommand request, CancellationToken cancellationToken)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request), "UpdateSectionCommand cannot be null.");
            }
            if (request.Request == null)
            {
                throw new ArgumentNullException(nameof(request.Request), "SectionUpdateRequest cannot be null.");
            }
            return await sectionService.UpdateSectionAsync(request.Request);
        }
    }
}
