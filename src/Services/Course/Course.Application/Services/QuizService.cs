using Course.Application.Dtos.QuizDto;

namespace Course.Application.Services
{
    public class QuizService(IUnitOfWork unitOfWork, ILogger<QuizService> logger)
        : IQuizService
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
                await unitOfWork.RollbackTransactionAsync();
                logger.LogError(ex, "An error occurred while executing the transaction.");
                throw;
            }
        }

        public async Task<QuizResponse> AddQuizAsync(QuizAddRequest request)
        {
            if (request == null)
            {
                logger.LogError("Quiz request cannot be null.");
                throw new ArgumentNullException(nameof(request), "Quiz request cannot be null.");
            }

            logger.LogInformation("Adding a new quiz with title: {Title}", request.Title);

            var lecture = await unitOfWork.Repository<Lecture>()
                .GetByAsync(x => x.Id == request.LectureId)
                ?? throw new LectureNotFoundException($"Lecture with id {request.LectureId} Not Found");

            var quiz = request.Adapt<Quiz>();
            quiz.CreatedAt = DateTime.UtcNow;

            await ExecuteWithTransactionAsync(async () =>
            {
                logger.LogInformation("Creating quiz for lecture: {LectureTitle}", lecture.Title);
                await unitOfWork.Repository<Quiz>().CreateAsync(quiz);
            });
            return quiz.Adapt<QuizResponse>();
        }

        public async Task<QuizResponse> GetQuizByIdAsync(Guid id)
        {
            var quiz = await unitOfWork.Repository<Quiz>()
                .GetByAsync(x => x.Id == id, includeProperties: "Lecture")
                ?? throw new QuizNotFoundException($"Quiz with id {id} not found.");

            return quiz.Adapt<QuizResponse>(); ;
        }

        public async Task<IEnumerable<QuizResponse>> GetQuizzesByAsync(Expression<Func<Quiz, bool>>? filter = null)
        {
            var quizzes = await unitOfWork.Repository<Quiz>()
                .GetAllAsync(filter, includeProperties: "Lecture");

            if (quizzes == null || !quizzes.Any())
            {
                logger.LogInformation("No quizzes found matching the provided filter.");
                return Enumerable.Empty<QuizResponse>();
            }

            return quizzes.Adapt<IEnumerable<QuizResponse>>(); ;
        }

        public async Task<QuizResponse> UpdateQuizAsync(QuizUpdateRequest request)
        {
            if (request == null)
            {
                logger.LogError("Quiz update request cannot be null.");
                throw new ArgumentNullException(nameof(request), "Quiz update request cannot be null.");
            }

            _ = await unitOfWork.Repository<Lecture>()
                .GetByAsync(x => x.Id == request.LectureId)
                ?? throw new LectureNotFoundException($"Lecture with id {request.LectureId} Not Found");

            var existingQuiz = await unitOfWork.Repository<Quiz>()
                .GetByAsync(x => x.Id == request.Id, includeProperties: "Lecture")
                ?? throw new QuizNotFoundException($"Quiz with id {request.Id} not found.");

            request.Adapt(existingQuiz);

            await ExecuteWithTransactionAsync(async () =>
            {
                await unitOfWork.Repository<Quiz>().UpdateAsync(existingQuiz);
            });
            return existingQuiz.Adapt<QuizResponse>(); ;
        }

        public async Task<bool> DeleteQuizAsync(Guid id)
        {
            var quiz = await unitOfWork.Repository<Quiz>()
                .GetByAsync(x => x.Id == id)
                ?? throw new QuizNotFoundException($"Quiz with id {id} not found.");

            await ExecuteWithTransactionAsync(async () =>
            {
                await unitOfWork.Repository<Quiz>().DeleteAsync(quiz);
            });

            return true;
        }
    }
}