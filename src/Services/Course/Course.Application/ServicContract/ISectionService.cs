using Course.Application.Dtos.SectionDto;

namespace Course.Application.ServicContract
{
    public interface ISectionService
    {
        Task<SectionResponse> AddSectionAsync(SectionAddRequest? sectionAddRequest);
        Task<SectionResponse> UpdateSectionAsync(SectionUpdateRequest? sectionUpdateRequest);
        Task<bool> DeleteSectionAsync(Guid sectionId);  
        Task<SectionResponse> GetSectionByAsync(Expression<Func<Section, bool>> expression);
        Task<IEnumerable<SectionResponse>> GetAllSectionsAsync(Expression<Func<Section, bool>>? expression = null);

    }
}
