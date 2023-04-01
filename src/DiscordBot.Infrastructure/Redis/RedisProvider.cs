using System.Text.Json;
using DiscordBot.Application.Services.Redis;

namespace DiscordBot.Infrastructure.Redis;

public class RedisProvider : IRedisProvider
{
    private readonly IRedisConnectionMultiplexer _multiplexer;
    private readonly JsonSerializerOptions _serializerOptions;

    public RedisProvider(IRedisConnectionMultiplexer multiplexer,
        JsonSerializerOptions serializerOptions)
    {
        _multiplexer = multiplexer;
        _serializerOptions = serializerOptions;
    }
    
    public async Task SaveAsync<T>(string cacheKey, T value, TimeSpan? expiry = null)
    {
        var json = SerializeValue(value);
        var db = await _multiplexer.GetDatabaseAsync();
        await db.StringSetAsync(cacheKey, json, expiry);
    }

    public async Task<Dictionary<string, TValue?>> GetFromHashAllAsync<TValue>(string outerKey)
    {
        var db = await _multiplexer.GetDatabaseAsync();
        var result = await db.HashGetAllAsync(outerKey);
        return result.ToDictionary(x => x.Name.ToString(), x => DeserializeValue<TValue>(x.Value));
    }

    public async Task AddToHashAsync<T>(string outerKey, string innerKey, T value)
    {
        var db = await _multiplexer.GetDatabaseAsync();
        await db.HashSetAsync(outerKey, innerKey, SerializeValue(value));
    }

    public async Task RemoveFromHashAsync(string outerKey, string innerKey)
    {
        var db = await _multiplexer.GetDatabaseAsync();
        await db.HashDeleteAsync(outerKey, innerKey);
    }
    
    public async Task<T?> GetAsync<T>(string cacheKey)
    {
        var db = await _multiplexer.GetDatabaseAsync();
        var result = await db.StringGetAsync(cacheKey);
        if (result.HasValue)
        {
            return DeserializeValue<T>(result);
        }

        return default;
    }
    
    public async Task<bool> ExistsAsync(string cacheKey)
    {
        var db = await _multiplexer.GetDatabaseAsync();
        return await db.KeyExistsAsync(cacheKey);
    }

    private string SerializeValue<T>(T value) => JsonSerializer.Serialize(value, _serializerOptions);
    
    private T? DeserializeValue<T>(string? json) =>
        json is not null ? JsonSerializer.Deserialize<T>(json, _serializerOptions) : default;
}