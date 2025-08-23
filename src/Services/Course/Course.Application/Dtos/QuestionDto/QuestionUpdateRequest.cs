using Course.Application.Dtos.AnswerDto;
using Course.Domain.Enums;

namespace Course.Application.Dtos.QuestionDto
{
    public class QuestionUpdateRequest
    {
        public Guid Id { get; set; }
        public string QuestionText { get; set; }
        public int Marks { get; set; }
        public int Order { get; set; }
        public List<AnswerUpdateRequest> Answers { get; set; } = new();
    }
}
