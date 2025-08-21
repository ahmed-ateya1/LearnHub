namespace Course.Application.Dtos.QuizDto
{
    public class QuizAddRequest
    {
        public string Title { get; set; }
        public int TotalMarks { get; set; }
        public int PassingMarks { get; set; }
        public int? TimeLimitInMinutes { get; set; } = null;
        public Guid LectureId { get; set; }
        public Guid CreatedBy { get; set; }

    }
}
