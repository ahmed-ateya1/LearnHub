using Course.Application.Course.Commands.CreateCourse;
using Course.Application.Course.Commands.DeleteCourse;
using Course.Application.Course.Commands.UpdateCourse;
using Course.Application.Course.Commands.UpdateCourseStatus;
using Course.Application.Course.Queries.GetAllCourses;
using Course.Application.Course.Queries.GetCourseById;
using Course.Application.Course.Queries.GetCOurseByName;
using Course.Application.Course.Queries.GetCoursesByCategoryId;
using Course.Application.Course.Queries.GetCoursesByUserId;
using Course.Application.Dtos.CourseDto;
using Course.Domain.Enums;
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
            if (response == null)
            {
                return BadRequest("Failed to create course.");
            }
            return Ok(response);
        }
        [HttpPut("update")]
        public async Task<IActionResult> UpdateCourse([FromForm] CourseUpdateRequest course)
        {
            var command = new UpdateCourseCommand(course);
            var response = await sender.Send(command);
            if (response == null)
            {
                return BadRequest("Failed to update course.");
            }
            return Ok(response);
        }
        [HttpDelete("delete/{courseId}")]
        public async Task<IActionResult> DeleteCourse(Guid courseId)
        {
            var command = new DeleteCourseCommand(courseId);
            var response = await sender.Send(command);
            if (!response)
            {
                return BadRequest($"Failed to delete course with ID {courseId}.");
            }
            return Ok($"Course with ID {courseId} deleted successfully.");
        }
        [HttpGet("get-all")]
        public async Task<IActionResult> GetAllCourses()
        {
            var query = new GetAllCoursesQuery();
            var response = await sender.Send(query);
            if (response == null || !response.Any())
            {
                return NotFound("No courses found.");
            }
            return Ok(response);
        }
        [HttpGet("get-by-id/{courseId}")]
        public async Task<IActionResult> GetCourseById(Guid courseId)
        {
            var query = new GetCourseByIdQuery(courseId);
            var response = await sender.Send(query);
            if (response == null)
            {
                return NotFound($"Course with ID {courseId} not found.");
            }
            return Ok(response);
        }
        [HttpGet("get-by-name/{courseName}")]
        public async Task<IActionResult> GetCourseByName(string courseName)
        {
            var query = new GetCourseByNameQuery(courseName);
            var response = await sender.Send(query);
            if (response == null || !response.Any())
            {
                return NotFound($"No courses found with name containing '{courseName}'.");
            }
            return Ok(response);
        }
        [HttpGet("get-by-instructor/{instructorId}")]
        public async Task<IActionResult> GetCoursesByInstructorId(Guid instructorId)
        {
            var query = new GetCoursesByUserIdQuery(instructorId);
            var response = await sender.Send(query);
            if (response == null || !response.Any())
            {
                return NotFound($"No courses found for instructor with ID {instructorId}.");
            }
            return Ok(response);
        }
        [HttpGet("get-by-category/{categoryId}")]
        public async Task<IActionResult> GetCoursesByCategoryId(Guid categoryId)
        {
            var query = new GetCoursesByCategoryQuery(categoryId);
            var response = await sender.Send(query);
            if (response == null || !response.Any())
            {
                return NotFound($"No courses found in category with ID {categoryId}.");
            }
            return Ok(response);
        }
        [HttpPatch("update-status/{courseId}")]
        public async Task<IActionResult> UpdateCourseStatus(Guid courseId, [FromQuery] CourseStatus status)
        {
            var command = new UpdateCourseStatusCommand(courseId, status);
            var response = await sender.Send(command);
            if (!response)
            {
                return BadRequest($"Failed to update status for course with ID {courseId}.");
            }
            return Ok(response);
        }
    }
}
