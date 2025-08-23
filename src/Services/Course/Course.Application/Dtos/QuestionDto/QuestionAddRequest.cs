using Course.Application.Dtos.AnswerDto;
using Course.Domain.Enums;

namespace Course.Application.Dtos.QuestionDto
{
    public class QuestionAddRequest
    {
        public string QuestionText { get; set; }
        public QuestionType QuestionType { get; set; }
        public int Marks { get; set; }
        public int Order { get; set; }
        public Guid QuizId { get; set; }
        public List<AnswerAddRequest> Answers { get; set; } = new();

    }
}
