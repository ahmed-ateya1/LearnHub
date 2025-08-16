using Course.Application.Dtos.SectionDto;

namespace Course.Application.Services
{
    public class SectionService(IUnitOfWork unitOfWork , ILogger<SectionService> logger)
        : ISectionService
    {
        private async Task ExecuteWithTransactionAsync(Func<Task> action)
        {
            using var transaction = await unitOfWork.BeginTransactionAsync();
            try
            {
                await action();
                await unitOfWork.CommitTransactionAsync();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while executing transaction");
                await unitOfWork.RollbackTransactionAsync();
                throw;
            }
        }
        public async Task<SectionResponse> AddSectionAsync(SectionAddRequest? sectionAddRequest)
        {
            if (sectionAddRequest == null)
            {
                logger.LogError("Section add request cannot be null.");
                throw new ArgumentNullException(nameof(sectionAddRequest), "Section add request cannot be null.");
            }

            var section = sectionAddRequest.Adapt<Section>();

            logger.LogInformation("Adding section with title: {Title}", section.Title);

            await ExecuteWithTransactionAsync(async () =>
            {
                var sectionRepository = unitOfWork.Repository<Section>();
                await sectionRepository.CreateAsync(section);
            });
            logger.LogInformation("Section added successfully with ID: {Id}", section.Id);
            return section.Adapt<SectionResponse>();
        }

        public async Task<IEnumerable<SectionResponse>> GetAllSectionsAsync(Expression<Func<Section, bool>>? expression = null)
        {
            var sections = await unitOfWork.Repository<Section>()
                .GetAllAsync(expression);
            logger.LogInformation("Retrieved {Count} sections", sections?.Count() ?? 0);
            if (sections == null || !sections.Any())
            {
                logger.LogWarning("No sections found matching the provided criteria.");
                return Enumerable.Empty<SectionResponse>();
            }
            logger.LogInformation("Found {Count} sections", sections.Count());
            return sections.Adapt<IEnumerable<SectionResponse>>();
        }

        public async Task<SectionResponse> GetSectionByAsync(Expression<Func<Section, bool>> expression)
        {
            var section = await unitOfWork.Repository<Section>()
                .GetByAsync(expression);

            if (section == null)
            {
                logger.LogWarning("Section not found for the provided expression.");
                throw new KeyNotFoundException("Section not found.");
            }
            logger.LogInformation("Section found with ID: {Id}", section.Id);
            return section.Adapt<SectionResponse>();
        }

        public async Task<SectionResponse> UpdateSectionAsync(SectionUpdateRequest? sectionUpdateRequest)
        {
            if (sectionUpdateRequest == null)
            {
                logger.LogError("Section update request cannot be null.");
                throw new ArgumentNullException(nameof(sectionUpdateRequest), "Section update request cannot be null.");
            }

            logger.LogInformation("Updating section with ID: {Id}", sectionUpdateRequest.Id);
            var section = await unitOfWork.Repository<Section>()
                .GetByAsync(s => s.Id == sectionUpdateRequest.Id);

            if (section == null)
            {
                logger.LogWarning("Section not found for ID: {Id}", sectionUpdateRequest.Id);
                throw new KeyNotFoundException("Section not found.");
            }

            sectionUpdateRequest.Adapt(section);

            logger.LogInformation("Mapped section update request to section entity: {Section}", section);
            await ExecuteWithTransactionAsync(async () =>
            {
                var sectionRepository = unitOfWork.Repository<Section>();
                await sectionRepository.UpdateAsync(section);
            });
            logger.LogInformation("Section updated successfully with ID: {Id}", section.Id);
            return section.Adapt<SectionResponse>();
        }

        public async Task<bool> DeleteSectionAsync(Guid sectionId)
        {
            var section = await unitOfWork.Repository<Section>()
                .GetByAsync(s => s.Id == sectionId);

            if (section == null)
            {
                logger.LogWarning("Section not found for ID: {Id}", sectionId);
                throw new KeyNotFoundException("Section not found.");
            }

            await ExecuteWithTransactionAsync(async () =>
            { 
                await unitOfWork.Repository<Section>()
                .DeleteAsync(section);
            });
            logger.LogInformation("Section deleted successfully with ID: {Id}", sectionId);

            return true;
        }
    }
}
