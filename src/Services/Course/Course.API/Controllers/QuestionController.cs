using Course.Application.Dtos.QuestionDto;
using Course.Application.Slices.Questions.Command.AddQuestion;
using Course.Application.Slices.Questions.Command.DeleteQuestion;
using Course.Application.Slices.Questions.Command.UpdateQuestion;
using Course.Application.Slices.Questions.Queries.GetQuestionById;
using Course.Application.Slices.Questions.Queries.GetQuestions;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Course.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuestionController(ISender sender)
        : ControllerBase
    {
        [HttpGet("{quizId}")]
        public async Task<IActionResult> GetQuestions(Guid quizId, CancellationToken cancellationToken)
        {
            var query = new GetQuestionsQuery(quizId);
            var questions = await sender.Send(query, cancellationToken);
            return Ok(questions);
        }
        [HttpGet("question/{id}")]
        public async Task<IActionResult> GetQuestionById(Guid id, CancellationToken cancellationToken)
        {
            var query = new GetQuestionByIdCommand(id);
            var question = await sender.Send(query, cancellationToken);
            return Ok(question);
        }
        [HttpPost]
        public async Task<IActionResult> AddQuestion([FromBody] QuestionAddRequest request, CancellationToken cancellationToken)
        {
            if (request == null)
            {
                return BadRequest("Request cannot be null");
            }
            var command = new AddQuestionCommand(request);
            var result = await sender.Send(command, cancellationToken);
            return CreatedAtAction(nameof(GetQuestionById), new { id = result.Id }, result);
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteQuestion(Guid id, CancellationToken cancellationToken)
        {
            var command = new DeleteQuestionCommand(id);
            var result = await sender.Send(command, cancellationToken);
            if (result)
            {
                return NoContent();
            }
            return NotFound("Question not found");
        }
        [HttpPut]
        public async Task<IActionResult> UpdateQuestion([FromBody] QuestionUpdateRequest request, CancellationToken cancellationToken)
        {
            if (request == null)
            {
                return BadRequest("Request cannot be null");
            }
            var command = new UpdateQuestionCommand(request);
            var result = await sender.Send(command, cancellationToken);
            if (result != null)
            {
                return Ok(result);
            }
            return NotFound("Question not found");
        }

    }
}
