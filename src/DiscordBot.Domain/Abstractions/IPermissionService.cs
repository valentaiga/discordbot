using Discord;

namespace DiscordBot.Domain.Abstractions;

public interface IPermissionService
{
    Task AllowCommandsInTextChannel(ulong guildId, ulong textChannelId);
    Task ProhibitCommandsInTextChannel(ulong guildId, ulong textChannelId);
    ValueTask<bool> IsCommandAllowed(ulong guildId, ulong textChannelId, ChannelPermission requiredPermission);
}