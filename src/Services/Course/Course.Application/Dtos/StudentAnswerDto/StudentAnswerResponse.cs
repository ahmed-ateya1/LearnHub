namespace Course.Application.Dtos.StudentAnswerDto
{
    public class StudentAnswerResponse
    {
        public Guid Id { get; set; }
        public Guid StudentId { get; set; }
        public Guid QuestionId { get; set; }

        public Guid? SelectedAnswerId { get; set; }
        public string? AnswerText { get; set; }

        public bool IsCorrect { get; set; }
        public DateTime SubmittedAt { get; set; }
    }



}
