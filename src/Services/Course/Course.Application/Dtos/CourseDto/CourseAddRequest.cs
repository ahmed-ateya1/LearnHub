using Course.Domain.Enums;
using Microsoft.AspNetCore.Http;

namespace Course.Application.Dtos.CourseDto
{
    public class CourseAddRequest
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public IFormFile Poster { get; set; }
        public Guid InstructorId { get; set; }
        public CourseLevel CourseLevel { get; set; }    
        public string Language { get; set; }
        public Guid CategoryId { get; set; }
    }
}
