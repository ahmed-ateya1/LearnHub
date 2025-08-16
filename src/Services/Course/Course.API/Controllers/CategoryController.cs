using Course.Application.Dtos.CategoryDto;
using Course.Application.Slices.Categories.Commands.CreateCategory;
using Course.Application.Slices.Categories.Commands.DeleteCategory;
using Course.Application.Slices.Categories.Commands.UpdateCategory;
using Course.Application.Slices.Categories.Queries.GetBaseCategories;
using Course.Application.Slices.Categories.Queries.GetCategories;
using Course.Application.Slices.Categories.Queries.GetCategoriesByName;
using Course.Application.Slices.Categories.Queries.GetCategoryById;
using Course.Application.Slices.Categories.Queries.GetSubCategories;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Course.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController(ISender sender)
        : ControllerBase
    {
        [HttpPost("create")]
        public async Task<IActionResult> CreateCategory([FromBody] CategoryAddRequest category)
        {
            var command = new CreateCategoryCommand(category);
            var response = await sender.Send(command);
            if (response == null)
            {
                return BadRequest("Failed to create category.");
            }
            return Ok(response);
        }
        [HttpPut("update")]
        public async Task<IActionResult> UpdateCategory([FromBody] CategoryUpdateRequest category)
        {
            var command = new UpdateCategoryCommand(category);
            var response = await sender.Send(command);
            if (response == null)
            {
                return BadRequest("Failed to update category.");
            }
            return Ok(response);
        }
        [HttpDelete("delete/{categoryId}")]
        public async Task<IActionResult> DeleteCategory(Guid categoryId)
        {
            var command = new DeleteCategoryCommand(categoryId);
            var response = await sender.Send(command);
            if (!response)
            {
                return BadRequest($"Failed to delete category with ID {categoryId}.");
            }
            return Ok($"Category with ID {categoryId} deleted successfully.");
        }

        [HttpGet("get-by-id/{categoryId}")]
        public async Task<IActionResult> GetCategoryById(Guid categoryId)
        {
            var query = new GetCategoryByIdQuery(categoryId);
            var response = await sender.Send(query);
            if (response == null)
            {
                return NotFound($"Category with ID {categoryId} not found.");
            }
            return Ok(response);
        }
        [HttpGet("get-base-categories")]
        public async Task<IActionResult> GetBaseCategories()
        {
            var query = new GetBaseCategoriesQuery();
            var response = await sender.Send(query);
            if (response == null || !response.Any())
            {
                return NotFound("No base categories found.");
            }
            return Ok(response);
        }
        [HttpGet("get-sub-categories/{categoryId}")]
        public async Task<IActionResult> GetSubCategories(Guid categoryId)
        {
            var query = new GetSubCategoriesQuery(categoryId);
            var response = await sender.Send(query);
            if (response == null || !response.Any())
            {
                return NotFound($"No sub-categories found for category ID {categoryId}.");
            }
            return Ok(response);
        }
        [HttpGet("get-all")]
        public async Task<IActionResult> GetAllCategories()
        {
            var query = new GetCategoriesQuery();
            var response = await sender.Send(query);
            if (response == null || !response.Any())
            {
                return NotFound("No categories found.");
            }
            return Ok(response);

        }
        [HttpGet("get-by-name/{name}")]
        public async Task<IActionResult> GetCategoryByName(string name)
        {
            var query = new GetCategoriesByNameQuery(name);
            var response = await sender.Send(query);
            if (response == null)
            {
                return NotFound($"Category with name '{name}' not found.");
            }
            return Ok(response);
        }
        
    }
}
