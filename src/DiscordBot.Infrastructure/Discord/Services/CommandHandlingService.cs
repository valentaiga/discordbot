using Discord;
using Discord.Commands;
using Discord.WebSocket;
using DiscordBot.Domain.Extensions;
using IResult = Discord.Commands.IResult;

namespace DiscordBot.Infrastructure.Discord.Services;

public class CommandHandlingService
{
    private readonly CommandService _commands;
    private readonly DiscordSocketClient _client;
    private readonly IServiceProvider _services;

    public CommandHandlingService(CommandService commands, DiscordSocketClient client, IServiceProvider services)
    {
        _commands = commands;
        _client = client;
        _services = services;
    }

    public async Task CommandExecutedAsync(Optional<CommandInfo> command, ICommandContext context, IResult result)
    {
        // command is unspecified when there was a search failure (command not found); we don't care about these errors
        if (!command.IsSpecified)
        {
            var msg = await context.Channel.SendMessageAsync(
                "Unknown command.", messageReference: context.Message.Reference);
            await Task.Delay(4000);
            await msg.DeleteAsync().NoThrow();
            
            return;
        }

        await context.Message.DeleteAsync().NoThrow();

        // the command was successful, we don't care about this result, unless we want to log that a command succeeded.
        if (result.IsSuccess)
            return;

        // the command failed, notify the user that something happened.
        if (result is not ExecuteResult error)
        {
            var msg = await context.Channel.SendMessageAsync($"`{result.ErrorReason}`");
            await Task.Delay(4000);
            await msg.DeleteAsync().NoThrow();
            return;
        }

        // unexpected failure, show the stacktrace for future investigation.
        await context.Channel.SendMessageAsync($@"error: `{error.Exception.Message}`
```
{error.Exception.StackTrace}
```");
    }

    public async Task MessageReceivedAsync(SocketMessage rawMsg)
    {
        if (rawMsg is not SocketUserMessage { Source: MessageSource.User } message)
            return;

        var argPos = 0;
        if (!message.HasStringPrefix("t!", ref argPos))
            return;

        var context = new SocketCommandContext(_client, message);
        await _commands.ExecuteAsync(context, argPos, _services);
    }
}