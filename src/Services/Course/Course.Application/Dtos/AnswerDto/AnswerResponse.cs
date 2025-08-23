namespace Course.Application.Dtos.AnswerDto
{
    public class AnswerResponse
    {
        public Guid Id { get; set; }
        public string AnswerText { get; set; }
        public bool IsCorrect { get; set; }
        public int Order { get; set; }
        public Guid QuestionId { get; set; }
    }
}
