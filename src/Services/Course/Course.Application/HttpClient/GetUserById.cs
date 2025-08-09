using Course.Application.HttpClient;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;

public class GetUserById(
    IHttpClientFactory httpClientFactory,
    IDistributedCache distributedCache,
    ILogger<GetUserById> logger) : IGetUserById
{
    public async Task<UserDto?> ExecuteAsync(Guid userId)
    {
        string cacheKey = $"User_{userId}";

        var cachedUser = await distributedCache.GetStringAsync(cacheKey);
        if (cachedUser != null)
        {
            logger.LogInformation("Returning cached user {UserId}", userId);
            return JsonSerializer.Deserialize<UserDto>(cachedUser);
        }

        var httpClient = httpClientFactory.CreateClient("UserService");
        var response = await httpClient.GetAsync($"/api/users/{userId}");

        if (response.StatusCode == HttpStatusCode.NotFound)
            return null;

        if (!response.IsSuccessStatusCode)
        {
            logger.LogWarning("Failed to fetch user {UserId}, Status: {Status}", userId, response.StatusCode);
            throw new HttpRequestException($"Error fetching user: {response.StatusCode}");
        }

        var user = await response.Content.ReadFromJsonAsync<UserDto>();
        if (user == null) return null;

        var options = new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30)
        };
        await distributedCache.SetStringAsync(cacheKey, JsonSerializer.Serialize(user), options);

        logger.LogInformation("User {UserId} fetched and cached", userId);
        return user;
    }
}
