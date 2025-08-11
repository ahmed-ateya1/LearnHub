using Course.Application.Course.Commands.CreateCourse;
using Course.Application.Course.Queries.GetAllCourses;
using Course.Application.Dtos.CourseDto;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Course.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CourseController(ISender sender)
        : ControllerBase
    {
        [HttpPost("create")]
        public async Task<IActionResult> CreateCourse([FromForm] CourseAddRequest course)
        {
            var command = new CreateCourseCommand(course);
            var response = await sender.Send(command);
            return Ok(response);
        }
        [HttpGet("get-all")]
        public async Task<IActionResult> GetAllCourses()
        {
            var query = new GetAllCoursesQuery();
            var response = await sender.Send(query);
            return Ok(response);
        }
    }
}
