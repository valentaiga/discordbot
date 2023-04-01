using Discord;
using Discord.Commands;
using Discord.WebSocket;
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
            return;

        // the command was successful, we don't care about this result, unless we want to log that a command succeeded.
        if (result.IsSuccess)
            return;

        // the command failed, let's notify the user that something happened.
        if (result is ExecuteResult error)
        {
            await context.Channel.SendMessageAsync($@"error: `{error.Exception.Message}`
```
{error.Exception.StackTrace}
```");
            return;
        }
        
        await context.Channel.SendMessageAsync($"error: `{result.Error}:{result.ErrorReason}`");
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