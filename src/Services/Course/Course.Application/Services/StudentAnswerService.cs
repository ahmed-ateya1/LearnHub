using BuildingBlocks.Exceptions;
using Course.Application.Dtos.StudentAnswerDto;

namespace Course.Application.Services
{
    public class StudentAnswerService(IUnitOfWork unitOfWork, ILogger<StudentAnswerService> logger)
        : IStudentAnswerService
    {

        private async Task ExecuteWithTransactionAsync(Func<Task> action)
        {
            var transaction = await unitOfWork.BeginTransactionAsync();    
            try
            {
                await action();
                await transaction.CommitAsync();
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                logger.LogError(ex, "An error occurred during transaction.");
                throw;
            }
        }
        public async Task<StudentAnswerResponse> AddStudentAnswerAsync(StudentAnswerAddRequest request)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            var question = await unitOfWork.Repository<Question>()
                .GetByAsync(x=>x.Id == request.QuestionId,includeProperties:"Answers");

            if (question == null)
            {
                throw new QuestionNotFoundException($"Question with ID {request.QuestionId} not found.");
            }
            var studentAnswer = request.Adapt<StudentAnswer>();

           

            studentAnswer.SubmittedAt = DateTime.UtcNow;

            if (request.SelectedAnswerId.HasValue)
            {
                var selectedAnswer = question.Answers.FirstOrDefault(a => a.Id == request.SelectedAnswerId.Value);
                if (selectedAnswer == null)
                {
                    throw new NotFoundException($"Answer with ID {request.SelectedAnswerId} not found for Question ID {request.QuestionId}.");
                }
                studentAnswer.SelectedAnswerID = selectedAnswer.Id;
                studentAnswer.IsCorrect = selectedAnswer.IsCorrect;
            }
            else if (!string.IsNullOrWhiteSpace(request.AnswerText))
            {
               
                studentAnswer.IsCorrect = false;
                
            }
            else
            {
                throw new StudentAnswerNotFoundException("Either SelectedAnswerId or AnswerText must be provided.");
            }


            await ExecuteWithTransactionAsync(async () =>
            {
                await unitOfWork.Repository<StudentAnswer>().CreateAsync(studentAnswer);
            });

            return studentAnswer.Adapt<StudentAnswerResponse>();
        }

        public async Task<bool> DeleteStudentAnswerAsync(Guid id)
        {
             var existingAnswer = await unitOfWork.Repository<StudentAnswer>().GetByAsync(sa => sa.Id == id);

            if (existingAnswer == null)
            {
                throw new StudentAnswerNotFoundException($"Student answer with ID {id} not found.");
            }

            await ExecuteWithTransactionAsync(async () =>
            {
                await unitOfWork.Repository<StudentAnswer>().DeleteAsync(existingAnswer);
            });

            return true;
        }

        public async Task<IEnumerable<StudentAnswerResponse>> GetAllStudentAnswersAsync(Expression<Func<StudentAnswer, bool>>? filter = null)
        {
            var studentAnswers = await  unitOfWork.Repository<StudentAnswer>().GetAllAsync(filter: filter,includeProperties: "Question");

            if (studentAnswers == null || !studentAnswers.Any())
            {
                logger.LogInformation("No student answers found.");
                return Enumerable.Empty<StudentAnswerResponse>();
            }
            return studentAnswers.Adapt<IEnumerable<StudentAnswerResponse>>();
        }

        public async Task<StudentAnswerResponse> GetStudentAnswerByIdAsync(Guid id)
        {
            var studentAnswer = await  unitOfWork.Repository<StudentAnswer>().GetByAsync(sa => sa.Id == id,includeProperties: "Question");

            if (studentAnswer == null)
            {
                throw new StudentAnswerNotFoundException($"Student answer with ID {id} not found.");
            }

            return studentAnswer.Adapt<StudentAnswerResponse>();
        }

        public async Task<StudentAnswerResponse> UpdateStudentAnswerAsync(StudentAnswerUpdateRequest request)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            var existingAnswer = await unitOfWork.Repository<StudentAnswer>()
                .GetByAsync(sa => sa.Id == request.Id, includeProperties: "Question.Answers");

            if (existingAnswer == null)
                throw new StudentAnswerNotFoundException($"Student answer with ID {request.Id} not found.");

            request.Adapt(existingAnswer);
            existingAnswer.SubmittedAt = DateTime.UtcNow;

            if (request.SelectedAnswerId.HasValue)
            {
                var selectedAnswer = existingAnswer.Question.Answers
                    .FirstOrDefault(a => a.Id == request.SelectedAnswerId.Value);

                if (selectedAnswer == null)
                    throw new NotFoundException($"Answer with ID {request.SelectedAnswerId} not found for Question ID {request.Id}.");

                existingAnswer.SelectedAnswerID = selectedAnswer.Id;
                existingAnswer.IsCorrect = selectedAnswer.IsCorrect;
            }
            else if (!string.IsNullOrWhiteSpace(request.AnswerText))
            {
                existingAnswer.IsCorrect = false;
            }

            await ExecuteWithTransactionAsync(async () =>
            {
                await unitOfWork.Repository<StudentAnswer>().UpdateAsync(existingAnswer);
            });

            return existingAnswer.Adapt<StudentAnswerResponse>();
        }

    }
}
