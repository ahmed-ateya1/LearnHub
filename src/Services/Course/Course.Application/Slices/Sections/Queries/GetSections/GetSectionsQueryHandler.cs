using Course.Application.Dtos.SectionDto;

namespace Course.Application.Slices.Sections.Queries.GetSections
{
    public record GetSectionsQuery : IQuery<IEnumerable<SectionResponse>>;
    internal class GetSectionsQueryHandler(ISectionService sectionService)
        : IQueryHandler<GetSectionsQuery, IEnumerable<SectionResponse>>
    {
        public async Task<IEnumerable<SectionResponse>> Handle(GetSectionsQuery request, CancellationToken cancellationToken)
        {
            return await sectionService.GetAllSectionsAsync();
        }
    }
}
