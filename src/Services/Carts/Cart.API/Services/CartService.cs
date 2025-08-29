using Cart.API.Models;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace Cart.API.Services
{
    public class CartService : ICartService
    {
        private readonly IDistributedCache _cache;

        public CartService(IDistributedCache cache)
        {
            _cache = cache;
        }

        private const string CartKeyPrefix = "cart_";
        public async Task AddItemAsync(Guid userId, CartItem item)
        {
            var cartKey = $"{CartKeyPrefix}{userId}";

            var cart = await GetCartAsync(userId) ?? new List<CartItem>();

            cart.Add(item);

            var data = JsonSerializer.SerializeToUtf8Bytes(cart);

            await _cache.SetAsync(cartKey, data, new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(2)
            });
        }

        public async Task ClearCartAsync(Guid userId)
        {
            var cartKey = $"{CartKeyPrefix}{userId}";
            await _cache.RemoveAsync(cartKey);
        }

        public async Task<List<CartItem>> GetCartAsync(Guid userId)
        {
            var cartKey = $"{CartKeyPrefix}{userId}";
            var data = await _cache.GetAsync(cartKey);
            return data == null ? null : JsonSerializer.Deserialize<List<CartItem>>(data);
        }
    }
}
