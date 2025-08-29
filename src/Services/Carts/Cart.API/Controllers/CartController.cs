using Cart.API.Models;
using Cart.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace Cart.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly ICartService _cartService;

        public CartController(ICartService cartService)
        {
            _cartService = cartService;
        }

        [HttpPost("{userId}/add")]
        public async Task<IActionResult> Add(Guid userId, CartItem item)
        {
            await _cartService.AddItemAsync(userId, item);
            return Ok("Item added to cart.");
        }

        [HttpGet("{userId}")]
        public async Task<IActionResult> Get(Guid userId)
        {
            var cart = await _cartService.GetCartAsync(userId);
            return Ok(cart ?? new List<CartItem>());
        }

        [HttpDelete("{userId}")]
        public async Task<IActionResult> Clear(Guid userId)
        {
            await _cartService.ClearCartAsync(userId);
            return Ok("Cart cleared.");
        }
    }
}
