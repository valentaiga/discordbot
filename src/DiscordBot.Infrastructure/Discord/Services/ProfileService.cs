using DiscordBot.Application.Services.Redis;
using DiscordBot.Domain.Abstractions;
using DiscordBot.Domain.Entities;

namespace DiscordBot.Infrastructure.Discord.Services;

public class ProfileService : IProfileService
{
    private readonly IRedisProvider _redisProvider;

    public ProfileService(IRedisProvider redisProvider)
    {
        _redisProvider = redisProvider;
    }

    public async Task UpdateAsync(ulong guildId, ulong userId, Action<UserProfile> update)
    {
        var key = GetKey(guildId, userId);
        var profile = await _redisProvider.GetAsync<UserProfile>(key) ?? new (userId);
        update(profile);
        await _redisProvider.SaveAsync(key, profile);
    }

    public async Task<UserProfile> GetAsync(ulong guildId, ulong userId)
    {
        var key = GetKey(guildId, userId);
        var profile = await _redisProvider.GetAsync<UserProfile>(key) ?? new(userId);
        return profile;
    }

    private static string GetKey(ulong guildId, ulong userId) => $"{guildId}:{userId}";
}