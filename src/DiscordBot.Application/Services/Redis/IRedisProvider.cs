namespace DiscordBot.Application.Services.Redis;
public interface IRedisProvider
{
    Task SaveAsync<T>(string cacheKey, T value, TimeSpan? expiry = null);
    Task<T?> GetAsync<T>(string cacheKey);
    Task<Dictionary<string, TValue?>> GetFromHashAllAsync<TValue>(string outerKey);
    Task AddToHashAsync<T>(string outerKey, string innerKey, T value);
    Task RemoveFromHashAsync(string outerKey, string innerKey);
    Task<bool> ExistsAsync(string cacheKey);
}