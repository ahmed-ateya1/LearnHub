namespace Course.Application.Dtos.QuizDto
{
    public class QuizUpdateRequest
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public int TotalMarks { get; set; }
        public int PassingMarks { get; set; }
        public int? TimeLimitInMinutes { get; set; } = null;
        public Guid LectureId { get; set; }
        public Guid CreatedBy { get; set; }

    }
}
