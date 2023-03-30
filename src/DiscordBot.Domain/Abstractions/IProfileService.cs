using DiscordBot.Domain.Entities;

namespace DiscordBot.Domain.Abstractions;

public interface IProfileService
{
    Task UpdateAsync(ulong guildId, ulong userId, Action<UserProfile> update);
    Task<UserProfile> GetAsync(ulong guildId, ulong userId);
}