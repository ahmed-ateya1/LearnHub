using Course.Application.Dtos.LectureDto;
using Course.Application.Slices.Lectures.Command.AddLecture;
using Course.Application.Slices.Lectures.Command.DeleteLecture;
using Course.Application.Slices.Lectures.Command.UpdateLecture;
using Course.Application.Slices.Lectures.Queries.GetLectureById;
using Course.Application.Slices.Lectures.Queries.GetLectures;
using Course.Application.Slices.Lectures.Queries.GetLecturesBySection;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Course.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LectureController(ISender sender) : ControllerBase
    {
        [HttpPost("add")]
        public async Task<IActionResult> Add([FromForm] LectureAddRequest request)
        {
            var response = await sender.Send(new AddLectureCommand(request));

            if (response == null)
            {
                return BadRequest("Failed to add lecture.");
            }
            return Ok(response);
        }

        [HttpPut("update")]
        public async Task<IActionResult> Update([FromForm] LectureUpdateRequest request)
        {
            var response = await sender.Send(new UpdateLectureCommand(request));

            if (response == null)
            {
                return BadRequest("Failed to update lecture.");
            }
            return Ok(response);
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await sender.Send(new DeleteLectureCommand(id));
            if (!result)
            {
                return NotFound("Lecture not found or could not be deleted.");
            }

            return Ok(result);
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var response = await sender.Send(new GetLectureByIdQuery(id));
            if (response == null)
            {
                return NotFound("Lecture not found.");
            }
            return Ok(response);
        }

        [HttpGet("section/{sectionId:guid}")]
        public async Task<IActionResult> GetBySection(Guid sectionId)
        {
            var response = await sender.Send(new GetLecturesBySectionQuery(sectionId));
            if (response == null || !response.Any())
            {
                return NotFound("No lectures found for the specified section.");
            }
            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var response = await sender.Send(new GetLecturesQuery());
            if (response == null || !response.Any())
            {
                return NotFound("No lectures found.");
            }
            return Ok(response);
        }
    }
}