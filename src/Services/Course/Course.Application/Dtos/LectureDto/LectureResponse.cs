namespace Course.Application.Dtos.LectureDto
{
    public class LectureResponse
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string VideoUrl { get; set; }
        public Guid SectionId { get; set; }
        public string SectionName { get; set; }
        public int QuizCount { get; set; }

    }
}