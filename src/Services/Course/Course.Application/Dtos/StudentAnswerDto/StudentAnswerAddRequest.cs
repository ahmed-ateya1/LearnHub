namespace Course.Application.Dtos.StudentAnswerDto
{
    public class StudentAnswerAddRequest
    {
        public Guid StudentId { get; set; }
        public Guid QuestionId { get; set; }

        public Guid? SelectedAnswerId { get; set; }

        public string? AnswerText { get; set; }
    }
}
