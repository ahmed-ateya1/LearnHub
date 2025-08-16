using Course.Application.Dtos.SectionDto;

namespace Course.Application.Slices.Sections.Queries.GetSectionById
{
    public record GetSectionByIdQuery(Guid SectionId) : IQuery<SectionResponse>;
    public class GetSectionByIdQueryHandler(ISectionService sectionService)
        : IQueryHandler<GetSectionByIdQuery, SectionResponse>
    {
        public async Task<SectionResponse> Handle(GetSectionByIdQuery request, CancellationToken cancellationToken)
        {
            if (request.SectionId == Guid.Empty)
            {
                throw new ArgumentException("Section ID cannot be empty.", nameof(request.SectionId));
            }
            return await sectionService.GetSectionByAsync(s => s.Id == request.SectionId);
        }
    }
}
