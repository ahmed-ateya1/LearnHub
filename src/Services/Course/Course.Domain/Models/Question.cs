using Course.Domain.Enums;

namespace Course.Domain.Models
{
    public class Question
    {
        public Guid Id { get; set; } 
        public string QuestionText { get; set; }
        public QuestionType QuestionType { get; set; }
        public int Marks { get; set; }
        public int Order { get; set; }

        public Guid QuizId { get; set; }
        public virtual Quiz Quiz { get; set; }

        public virtual ICollection<Answer> Answers { get; set; } = [];
        public virtual ICollection<StudentAnswer> StudentAnswers { get; set; } = [];
    }
}
