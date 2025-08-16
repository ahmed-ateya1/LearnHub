using Course.Application.Dtos.SectionDto;
using Course.Application.Slices.Sections.Command.AddSection;
using Course.Application.Slices.Sections.Command.DeleteSection;
using Course.Application.Slices.Sections.Command.UpdateSection;
using Course.Application.Slices.Sections.Queries.GetSectionByCourse;
using Course.Application.Slices.Sections.Queries.GetSectionById;
using Course.Application.Slices.Sections.Queries.GetSections;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Course.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SectionController(ISender sender)
        : ControllerBase
    {
        [HttpGet("get-all")]
        public async Task<IActionResult> GetAllSections()
        {
            var query = new GetSectionsQuery();
            var response = await sender.Send(query);
            if (response == null || !response.Any())
            {
                return NotFound("No sections found.");
            }
            return Ok(response);
        }
        [HttpGet("get-by-course-id/{courseId}")]
        public async Task<IActionResult> GetSectionsByCourseId(Guid courseId)
        {
            var query = new GetSectionByCourseQuery(courseId);
            var response = await sender.Send(query);
            if (response == null || !response.Any())
            {
                return NotFound($"No sections found for course ID {courseId}.");
            }
            return Ok(response);
        }
        [HttpGet("get-by-id/{sectionId}")]
        public async Task<IActionResult> GetSectionById(Guid sectionId)
        {
            var query = new GetSectionByIdQuery(sectionId);
            var response = await sender.Send(query);
            if (response == null)
            {
                return NotFound($"Section with ID {sectionId} not found.");
            }
            return Ok(response);
        }
        [HttpPost("create")]
        public async Task<IActionResult> CreateSection([FromBody] SectionAddRequest sectionAddRequest)
        {
            if (sectionAddRequest == null)
            {
                return BadRequest("Invalid section add request.");
            }
            var command = new AddSectionCommand(sectionAddRequest);
            var response = await sender.Send(command);
            if (response == null)
            {
                return BadRequest("Failed to create section.");
            }
            return Ok(response);
        }
        [HttpDelete("delete/{sectionId}")]
        public async Task<IActionResult> DeleteSection(Guid sectionId)
        {
            var command = new DeleteSectionCommand(sectionId);
            var response = await sender.Send(command);
            if (!response)
            {
                return BadRequest($"Failed to delete section with ID {sectionId}.");
            }
            return Ok($"Section with ID {sectionId} deleted successfully.");
        }
        [HttpPut("update")]
        public async Task<IActionResult> UpdateSection([FromBody] SectionUpdateRequest sectionUpdateRequest)
        {
            if (sectionUpdateRequest == null || sectionUpdateRequest.Id == Guid.Empty)
            {
                return BadRequest("Invalid section update request.");
            }
            var command = new UpdateSectionCommand(sectionUpdateRequest);
            var response = await sender.Send(command);
            if (response == null)
            {
                return NotFound($"Section with ID {sectionUpdateRequest.Id} not found.");
            }
            return Ok(response);
        }
        
       
    }
}
