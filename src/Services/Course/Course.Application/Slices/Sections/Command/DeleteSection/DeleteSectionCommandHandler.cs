namespace Course.Application.Slices.Sections.Command.DeleteSection
{
    public record DeleteSectionCommand(Guid SectionId):ICommand<bool>;
    internal class DeleteSectionCommandHandler(ISectionService sectionService)
        : ICommandHandler<DeleteSectionCommand, bool>
    {
        public async Task<bool> Handle(DeleteSectionCommand request, CancellationToken cancellationToken)
        {
         
            return await sectionService.DeleteSectionAsync(request.SectionId);
        }
    }
}
