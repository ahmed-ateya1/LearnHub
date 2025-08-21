using Course.Application.Dtos.QuizDto;
using Course.Application.Slices.Quizzes.Command.AddQuiz;
using Course.Application.Slices.Quizzes.Command.DeleteQuiz;
using Course.Application.Slices.Quizzes.Command.UpdateQuiz;
using Course.Application.Slices.Quizzes.Queries.GetQuizById;
using Course.Application.Slices.Quizzes.Queries.GetQuizzes;
using Course.Application.Slices.Quizzes.Queries.GetQuizzesByLecture;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Course.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuizController(ISender sender) : ControllerBase
    {
        [HttpPost("add")]
        public async Task<IActionResult> Add([FromBody] QuizAddRequest request)
        {
            var response = await sender.Send(new AddQuizCommand(request));
            return Ok(response);
        }

        [HttpPut("update")]
        public async Task<IActionResult> Update([FromBody] QuizUpdateRequest request)
        {
            var response = await sender.Send(new UpdateQuizCommand(request));
            return Ok(response);
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await sender.Send(new DeleteQuizCommand(id));
            return Ok(result);
        }
        [HttpGet("get-by-lecture/{lectureId:guid}")]
        public async Task<IActionResult> GetQuizzesByLecture(Guid lectureId)
        {
            var response = await sender.Send(new GetQuizzesByLectureQuery(lectureId));
            return response == null || !response.Any()
                ? NotFound("No quizzes found for the specified lecture.")
                : Ok(response);
        }

        [HttpGet("get-by-id/{id:guid}")]
        public async Task<IActionResult> GetQuizById(Guid id)
        {
            var response = await sender.Send(new GetQuizByIdQuery(id));
            return response == null
                ? NotFound("Quiz not found.")
                : Ok(response);
        }

        [HttpGet("get-all")]
        public async Task<IActionResult> GetAllQuizzes()
        {
            var response = await sender.Send(new GetQuizzesQuery());
            return response == null || !response.Any()
                ? NotFound("No quizzes found.")
                : Ok(response);
        }
    }
}