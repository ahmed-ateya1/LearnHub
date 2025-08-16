using Course.Application.Dtos.LectureDto;

namespace Course.Application.Services
{
    public class LectureService(
        IUnitOfWork unitOfWork,
        ILogger<LectureService> logger,
        IFileServices fileServices)
        : ILectureService
    {
        private async Task ExecuteWithTransaction(Func<Task> action)
        {
            using var transaction = await unitOfWork.BeginTransactionAsync();
            try
            {
                await action();
                await transaction.CommitAsync();
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                logger.LogError(ex, "An error occurred while executing the transaction.");
                throw;
            }
        }

      

        public async Task<LectureResponse> AddLectureAsync(LectureAddRequest request)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request), "Lecture request cannot be null.");

            logger.LogInformation("Adding lecture: {Title}", request.Title);

            var section = await unitOfWork.Repository<Section>()
                .GetByAsync(s => s.Id == request.SectionId)
                ?? throw new KeyNotFoundException("Section not found");

            var lecture = request.Adapt<Lecture>();

            if (request.Video != null)
            {
                lecture.VideoUrl = await fileServices.CreateFile(request.Video);
            }

            await ExecuteWithTransaction(async () =>
            {
                await unitOfWork.Repository<Lecture>().CreateAsync(lecture);
            });

            var saved = await unitOfWork.Repository<Lecture>()
                .GetByAsync(l => l.Id == lecture.Id, includeProperties: "Section,Quizzes")
                ?? lecture;

            if (saved.Section == null)
                saved.Section = section;

            var response = lecture.Adapt<LectureResponse>();
            logger.LogInformation("Lecture created successfully: {LectureId}", response.Id);
            return response;
        }

        public async Task<LectureResponse> UpdateLectureAsync(LectureUpdateRequest request)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            logger.LogInformation("Updating lecture: {LectureId}", request.Id);

            var lecture = await unitOfWork.Repository<Lecture>()
                .GetByAsync(l => l.Id == request.Id, includeProperties: "Section,Quizzes")
                ?? throw new KeyNotFoundException("Lecture not found");

            if (lecture.SectionId != request.SectionId)
            {
                _ = await unitOfWork.Repository<Section>()
                    .GetByAsync(s => s.Id == request.SectionId)
                    ?? throw new KeyNotFoundException("Section not found");
            }

            request.Adapt(lecture);

            await ExecuteWithTransaction(async () =>
            {
                if (request.Video != null)
                {
                    lecture.VideoUrl = await fileServices.UpdateFile(request.Video, lecture.VideoUrl);
                }
                await unitOfWork.Repository<Lecture>().UpdateAsync(lecture);
            });

            var updated = await unitOfWork.Repository<Lecture>()
                .GetByAsync(l => l.Id == lecture.Id, includeProperties: "Section,Quizzes")
                ?? lecture;

            var response =  updated.Adapt<LectureResponse>();
            logger.LogInformation("Lecture updated successfully: {LectureId}", response.Id);
            return response;
        }

        public async Task<bool> DeleteLectureAsync(Guid id)
        {
            logger.LogInformation("Deleting lecture: {LectureId}", id);

            var lecture = await unitOfWork.Repository<Lecture>()
                .GetByAsync(l => l.Id == id, includeProperties: "Quizzes,Section")
                ?? throw new KeyNotFoundException("Lecture not found");

            await ExecuteWithTransaction(async () =>
            {
               
                if (!string.IsNullOrWhiteSpace(lecture.VideoUrl))
                {
                    await fileServices.DeleteFile(lecture.VideoUrl);
                }

                await unitOfWork.Repository<Lecture>().DeleteAsync(lecture);
            });

            logger.LogInformation("Lecture deleted: {LectureId}", id);
            return true;
        }

        public async Task<LectureResponse> GetLectureByIdAsync(Expression<Func<Lecture, bool>> expression)
        {
            logger.LogInformation("Fetching lecture with filter.");

            var lecture = await unitOfWork.Repository<Lecture>()
                .GetByAsync(expression, includeProperties: "Section,Quizzes")
                ?? throw new KeyNotFoundException("Lecture not found");

            var response = lecture.Adapt<LectureResponse>();

            logger.LogInformation("Lecture fetched: {LectureId}", response.Id);
            return response;
        }

        public async Task<IEnumerable<LectureResponse>> GetLecturesByAsync(Expression<Func<Lecture, bool>>? expression = null)
        {
            var lectures = await unitOfWork.Repository<Lecture>()
                .GetAllAsync(expression, includeProperties: "Section,Quizzes");

            logger.LogInformation("Retrieved {Count} lectures", lectures?.Count() ?? 0);

            if (lectures == null || !lectures.Any())
                return Enumerable.Empty<LectureResponse>();

            return lectures.Adapt<IEnumerable<LectureResponse>>();
        }
    }
}