using Course.Application.Dtos.QuestionDto;
using Course.Domain.Models;

namespace Course.Application.Services
{
    public class QuestionService(IUnitOfWork unitOfWork, ILogger<QuestionService> logger)
        : IQuestionService
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
        public async Task<QuestionResponse> AddQuestionAsync(QuestionAddRequest request)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            var quiz = await unitOfWork.Repository<Quiz>()
                .GetByAsync(x => x.Id == request.QuizId);

            if (quiz == null)
            {
                throw new QuizNotFoundException("Quiz not found");
            }

            var question = request.Adapt<Question>();

            question.Id = Guid.NewGuid();

            foreach (var answer in question.Answers)
            {
                answer.Id = Guid.NewGuid();
                answer.QuestionId = question.Id;
            }

            await ExecuteWithTransactionAsync(async () =>
            {
                await unitOfWork.Repository<Question>().CreateAsync(question);
            });

           

            return question.Adapt<QuestionResponse>();
        }

        public async Task<bool> DeleteQuestionAsync(Guid id)
        {
            var question = await unitOfWork.Repository<Question>().GetByAsync(x => x.Id == id, includeProperties: "Answers");

            if (question == null)
            {
                throw new QuestionNotFoundException("Question not found");
            }

            await ExecuteWithTransactionAsync(async () =>
            {
                if(question.Answers != null && question.Answers.Any())
                {
                    await unitOfWork.Repository<Answer>().RemoveRangeAsync(question.Answers);
                }
                await unitOfWork.Repository<Question>().DeleteAsync(question);
            });
            return true;
        }

        public async Task<IEnumerable<QuestionResponse>> GetAllQuestionsAsync(Expression<Func<Question, bool>>? filter = null)
        {
            var questions = await unitOfWork.Repository<Question>()
                .GetAllAsync(filter,includeProperties: "Answers");

            if (questions == null || !questions.Any())
            {
                return Enumerable.Empty<QuestionResponse>();
            }

            var result =  questions.Adapt<IEnumerable<QuestionResponse>>();

            result = result.OrderBy(q => q.Order).ToList();

            return result;
        }

        public async Task<QuestionResponse> GetQuestionByIdAsync(Guid id)
        {
            var question = await unitOfWork.Repository<Question>()
                    .GetByAsync(x => x.Id == id,includeProperties: "Answers");
            if (question == null)
            {
                throw new QuestionNotFoundException("Question not found");
            }

            return question.Adapt<QuestionResponse>();
        }

        public async Task<QuestionResponse> UpdateQuestionAsync(QuestionUpdateRequest request)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            var existingQuestion = await unitOfWork.Repository<Question>()
                .GetByAsync(x => x.Id == request.Id);

            if (existingQuestion == null)
                throw new QuestionNotFoundException("Question not found");

            request.Adapt(existingQuestion);

            await ExecuteWithTransactionAsync(async () =>
            {
                await unitOfWork.Repository<Question>().UpdateAsync(existingQuestion);

                var existingAnswers = (await unitOfWork.Repository<Answer>()
                    .GetAllAsync(a => a.QuestionId == existingQuestion.Id))
                    .ToList();

                var updatedAnswers = request.Answers.Select(a => new Answer
                {
                    Id = a.Id == Guid.Empty ? Guid.NewGuid() : a.Id,
                    AnswerText = a.AnswerText,
                    IsCorrect = a.IsCorrect,
                    Order = a.Order,
                    QuestionId = existingQuestion.Id
                }).ToList();

                var answersToRemove = existingAnswers
                    .Where(ea => !updatedAnswers.Any(ua => ua.Id == ea.Id))
                    .ToList();

                if (answersToRemove.Any())
                    await unitOfWork.Repository<Answer>().RemoveRangeAsync(answersToRemove);

                foreach (var updated in updatedAnswers)
                {
                    var existingAnswer = existingAnswers.FirstOrDefault(ea => ea.Id == updated.Id);

                    if (existingAnswer != null)
                    {
                        existingAnswer.AnswerText = updated.AnswerText;
                        existingAnswer.IsCorrect = updated.IsCorrect;
                        existingAnswer.Order = updated.Order;

                        await unitOfWork.Repository<Answer>().UpdateAsync(existingAnswer);
                    }
                    else
                    {
                        await unitOfWork.Repository<Answer>().CreateAsync(updated);
                    }
                }
            });

            return existingQuestion.Adapt<QuestionResponse>();
        }

    }
}
