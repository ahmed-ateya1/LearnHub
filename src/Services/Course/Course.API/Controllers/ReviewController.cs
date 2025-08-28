using Course.Application.Dtos.ReviewDto;
using Course.Application.Slices.Reviews.Commands.AddReview;
using Course.Application.Slices.Reviews.Commands.DeleteReview;
using Course.Application.Slices.Reviews.Commands.UpdateReview;
using Course.Application.Slices.Reviews.Queries.GetReviewById;
using Course.Application.Slices.Reviews.Queries.GetReviewsByCourse;
using Course.Application.Slices.Reviews.Queries.GetReviewsByUser;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Course.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewController(ISender sender)
        : ControllerBase
    {
        [HttpGet("by-course/{courseId}")]
        public async Task<IActionResult> GetReviewsByCourseId(Guid courseId)
        {
            var query = new GetReviewsByCourseQuery(courseId);
            var reviews = await sender.Send(query);
            return Ok(reviews);
        }
        [HttpGet("by-user/{userId}")]
        public async Task<IActionResult> GetReviewsByUserId(Guid userId)
        {
            var query = new GetReviewsByUserQuery(userId);
            var reviews = await sender.Send(query);
            return Ok(reviews);
        }
        [HttpGet("{reviewId}")]
        public async Task<IActionResult> GetReviewById(Guid reviewId)
        {
            var query = new GetReviewByIdQuery(reviewId);
            var review = await sender.Send(query);
            return Ok(review);
        }
        [HttpPost("add")]
        public async Task<IActionResult> AddReview([FromBody] ReviewAddRequest request)
        {
            var command = new AddReviewCommand(request);
            var review = await sender.Send(command);
            return CreatedAtAction(nameof(GetReviewById), new { reviewId = review.Id }, review);
        }
        [HttpDelete("{reviewId}")]
        public async Task<IActionResult> DeleteReview(Guid reviewId)
        {
            var command = new DeleteReviewCommand(reviewId);
            await sender.Send(command);
            return NoContent();
        }
        [HttpPut("update")]
        public async Task<IActionResult> UpdateReview([FromBody] ReviewUpdateRequest request)
        {
            
            var command = new UpdateReviewCommand(request);
            var updatedReview = await sender.Send(command);
            return Ok(updatedReview);
        }
    }
}
