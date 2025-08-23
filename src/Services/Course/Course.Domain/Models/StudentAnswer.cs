namespace Course.Domain.Models
{
    public class StudentAnswer
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid StudentId { get; set; }

        public Guid QuestionId { get; set; }
        public virtual Question Question { get; set; }

        public Guid? SelectedAnswerID { get; set; } 
        public virtual Answer? Answer { get; set; }

        public string? AnswerText { get; set; } 
        public bool IsCorrect { get; set; }
        public DateTime SubmittedAt { get; set; }
    }
}
