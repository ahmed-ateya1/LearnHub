using Microsoft.AspNetCore.Http;

namespace Course.Application.Dtos.LectureDto
{
    public class LectureUpdateRequest
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public IFormFile? Video { get; set; }
        public Guid SectionId { get; set; }
    }
}