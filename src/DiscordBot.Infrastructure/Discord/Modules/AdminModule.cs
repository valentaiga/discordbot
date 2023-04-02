using Discord;
using Discord.Commands;
using DiscordBot.Domain.Abstractions;
using DiscordBot.Domain.Extensions;

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

    [Command("clear")]
    [Summary("clear last N message in text channel")]
    public async Task Clear(ushort count)
    {
        if (count > 50)
        {
            await ReplyAsync("Not allowed to clear more than 50 messages per command.");
            return;
        }

        var resultMessage = await Context.Channel.SendMessageAsync($"Starting to clear {count} messages.");
        var counter = 0;
        await foreach (var messages in Context.Channel.GetMessagesAsync(resultMessage.Id, Direction.Before, limit: count))
        {
            foreach (var msg in messages)
            {
                if (counter++ >= count) return;
                await msg.DeleteAsync().NoThrow();
                if (counter % 5 == 0)
                    await resultMessage.ModifyAsync(props =>
                        props.Content = $"Starting to clear {count} messages. ({counter} cleared)").NoThrow();
            }
        }

        await resultMessage.DeleteAsync().NoThrow();
    }
}