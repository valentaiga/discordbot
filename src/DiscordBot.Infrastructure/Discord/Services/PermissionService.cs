using Discord;
using DiscordBot.Domain.Abstractions;

namespace DiscordBot.Infrastructure.Discord.Services;

public class PermissionService : IPermissionService
{
    private readonly IGuildProvider _guildProvider;

    public PermissionService(IGuildProvider guildProvider)
    {
        _guildProvider = guildProvider;
    }

    public Task AllowCommandsInTextChannel(ulong guildId, ulong textChannelId) =>
        _guildProvider.Update(guildId, settings =>
        {
            settings.AllowedForCommandsTextChannels.Add(textChannelId);
        });

    public Task ProhibitCommandsInTextChannel(ulong guildId, ulong textChannelId) => 
        _guildProvider.Update(guildId, settings =>
        {
            settings.AllowedForCommandsTextChannels.Remove(textChannelId);
        });

    public async ValueTask<bool> IsCommandAllowed(ulong guildId,
        ulong textChannelId,
        ChannelPermission requiredPermission)
    {
        if (requiredPermission != ChannelPermission.SendMessages)
            return true;
        
        var settings = await _guildProvider.GetSettings(guildId);
        return settings.AllowedForCommandsTextChannels.Contains(textChannelId);
    }
}