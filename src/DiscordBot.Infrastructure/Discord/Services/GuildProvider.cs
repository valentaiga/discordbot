using DiscordBot.Application.Services.Redis;
using DiscordBot.Domain.Abstractions;
using DiscordBot.Domain.Entities;

namespace DiscordBot.Infrastructure.Discord.Services;

public class GuildProvider : IGuildProvider
{
    private readonly IRedisProvider _redisProvider;

    public GuildProvider(IRedisProvider redisProvider)
    {
        _redisProvider = redisProvider;
    }

    public async Task<GuildSettings> GetSettings(ulong guildId)
    {
        var key = GetKey(guildId);
        return await _redisProvider.GetAsync<GuildSettings>(key) 
               ?? new(guildId);
    }

    public async Task Update(ulong guildId, Action<GuildSettings> update)
    {
        var settings = await GetSettings(guildId);
        update(settings);
        var key = GetKey(guildId);
        await _redisProvider.SaveAsync(key, settings);
    }

    private static string GetKey(ulong guildId) => $"{guildId}:settings";
}