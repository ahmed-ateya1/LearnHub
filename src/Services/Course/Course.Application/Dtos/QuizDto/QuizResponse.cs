namespace Course.Application.Dtos.QuizDto
{
    public class QuizResponse
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public int TotalMarks { get; set; }
        public int PassingMarks { get; set; }
        public int? TimeLimitInMinutes { get; set; } = null;
        public DateTime CreatedAt { get; set; }

        public Guid CreatedBy { get; set; }
        public Guid LectureId { get; set; }
        public string LectureTitle { get; set; }
    }
}
