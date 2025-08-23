using Course.Application.Dtos.StudentAnswerDto;
using Course.Application.Slices.StudentAnswers.Command.AddStudentAnswer;
using Course.Application.Slices.StudentAnswers.Command.DeleteStudentAnswer;
using Course.Application.Slices.StudentAnswers.Command.UpdateStudentAnswer;
using Course.Application.Slices.StudentAnswers.Queries.GetAllStudentAnswer;
using Course.Application.Slices.StudentAnswers.Queries.GetStudentAnswerForQuiz;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Course.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentAnswerController(ISender sender)
        : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> AddStudentAnswer([FromBody] StudentAnswerAddRequest request)
        {
            var result = await sender.Send(new AddStudentAnswerCommand(request));
            if (result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }
        [HttpPut("update")]
        public async Task<IActionResult> AddStudentAnswer([FromBody] StudentAnswerUpdateRequest request)
        {
            var result = await sender.Send(new UpdateStudentAnswerCommand(request));
            if (result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStudentAnswer(Guid id)
        {
            var result = await sender.Send(new DeleteStudentAnswerCommand(id));
            if (!result)
            {
                return NotFound();
            }
            return Ok(result);
        }

        [HttpGet("quiz/{quizId}/student/{studentId}")]
        public async Task<IActionResult> GetAllStudentAnswers(Guid quizId, Guid studentId)
        {
            var result = await sender.Send(new GetStudentAnswerForQuizQuery(studentId, quizId));
            if (result == null || !result.Any())
            {
                return NotFound();
            }

            return Ok(result);
        }

        [HttpGet("quiz/{quizId}")]
        public async Task<IActionResult> GetAllStudentAnswersForQuiz(Guid quizId)
        {
            var result = await sender.Send(new GetAllStudentAnswerQuery(quizId));
            if (result == null || !result.Any())
            {
                return NotFound();
            }
            return Ok(result);
        }
    }
}
