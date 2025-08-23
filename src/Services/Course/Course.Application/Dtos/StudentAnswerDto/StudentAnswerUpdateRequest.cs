namespace Course.Application.Dtos.StudentAnswerDto
{
    public class StudentAnswerUpdateRequest
    {
        public Guid Id { get; set; } 
        public Guid? SelectedAnswerId { get; set; }
        public string? AnswerText { get; set; }
    }



}
