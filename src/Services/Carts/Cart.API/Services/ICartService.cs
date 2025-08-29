using Cart.API.Models;

namespace Cart.API.Services
{
    public interface ICartService
    {
        Task AddItemAsync(Guid userId, CartItem item);
        Task<List<CartItem>> GetCartAsync(Guid userId);
        Task ClearCartAsync(Guid userId);
    }
}
