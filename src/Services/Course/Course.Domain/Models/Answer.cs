namespace Course.Domain.Models
{
    public class Answer
    {
        public Guid Id { get; set; }
        public string AnswerText { get; set; }
        public bool IsCorrect { get; set; }
        public int Order { get; set; }

        public Guid QuestionId { get; set; }
        public virtual Question Question { get; set; }
    }
}
