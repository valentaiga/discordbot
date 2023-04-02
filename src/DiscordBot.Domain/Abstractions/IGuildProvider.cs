using DiscordBot.Domain.Entities;

namespace DiscordBot.Domain.Abstractions;

public interface IGuildProvider
{
    Task<GuildSettings> GetSettings(ulong guildId);
    Task Update(ulong guildId, Action<GuildSettings> update);
}