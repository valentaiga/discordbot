using Discord;
using Discord.Commands;
using Discord.WebSocket;
using IResult = Discord.Commands.IResult;

namespace DiscordBot.Infrastructure.Discord.Services;

public class CommandHandlingService
{
    private readonly IServiceProvider _services;
    private readonly CommandService _commands;
    private readonly DiscordSocketClient _client;

    public CommandHandlingService(IServiceProvider services)
    {
        _services = services;
        _commands = services.GetRequiredService<CommandService>();
        _client = services.GetRequiredService<DiscordSocketClient>();
        
        _client.MessageReceived += MessageReceivedAsync;
        _client.SlashCommandExecuted += ClientOnSlashCommandExecuted;
        
        _commands.CommandExecuted += CommandExecutedAsync;
    }

    private async Task CommandExecutedAsync(Optional<CommandInfo> command, ICommandContext context, IResult result)
    {
        // command is unspecified when there was a search failure (command not found); we don't care about these errors
        if (!command.IsSpecified)
            return;

        // the command was successful, we don't care about this result, unless we want to log that a command succeeded.
        if (result.IsSuccess)
            return;

        // the command failed, let's notify the user that something happened.
        await context.Channel.SendMessageAsync($"error: {result}");
    }

    private async Task MessageReceivedAsync(SocketMessage rawMsg)
    {
        if (rawMsg is not SocketUserMessage { Source: MessageSource.User } message)
            return;

        var argPos = 0;
        if (!message.HasStringPrefix("t!", ref argPos))
            return;
        
        var context = new SocketCommandContext(_client, message);
        await _commands.ExecuteAsync(context, argPos, _services);
    }

    private Task ClientOnSlashCommandExecuted(SocketSlashCommand rawCmd)
    {
        throw new NotImplementedException();
    }
}