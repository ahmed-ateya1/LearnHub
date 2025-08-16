using Course.Application.Dtos.SectionDto;

namespace Course.Application.Slices.Sections.Queries.GetSectionByCourse
{
    public record GetSectionByCourseQuery(Guid CourseId) : IQuery<IEnumerable<SectionResponse>>;
    public class GetSectionByCourseQueryHandler (ISectionService sectionService): 
        IQueryHandler<GetSectionByCourseQuery, IEnumerable<SectionResponse>>
    {
        public async Task<IEnumerable<SectionResponse>> Handle(GetSectionByCourseQuery request, CancellationToken cancellationToken)
        {
            return await sectionService.GetAllSectionsAsync(s => s.CourseId == request.CourseId);
        }
    }
}
