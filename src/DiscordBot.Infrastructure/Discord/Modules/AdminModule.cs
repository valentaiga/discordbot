using Discord.Commands;
using DiscordBot.Domain.Abstractions;

namespace DiscordBot.Infrastructure.Discord.Modules;

[RequireContext(ContextType.Guild)]
public class AdminModule : ModuleBase<SocketCommandContext>
{
    private readonly IPermissionService _permissionService;

    public AdminModule(IPermissionService permissionService)
    {
        _permissionService = permissionService;
    }

    [Command("+commands")]
    [Summary("allow to execute commands in text channel")]
    public async Task AllowCommandsInChannel()
    {
        var guildId = Context.Guild.Id;
        var textChannelId = Context.Channel.Id;
        await _permissionService.AllowCommandsInTextChannel(guildId, textChannelId);
        await ReplyAsync("Commands are allowed to execute in channel.");
    }
    
    [Command("-commands")]
    [Summary("prohibit to execute commands in text channel")]
    public async Task ProhibitCommandsInChannel()
    {
        var guildId = Context.Guild.Id;
        var textChannelId = Context.Channel.Id;
        await _permissionService.ProhibitCommandsInTextChannel(guildId, textChannelId);
        await ReplyAsync("Commands are prohibited to execute in channel.");
    }
}