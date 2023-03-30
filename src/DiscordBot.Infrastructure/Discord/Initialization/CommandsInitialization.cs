using Discord.Commands;
using Discord.WebSocket;
using DiscordBot.Domain.Primitives;
using DiscordBot.Infrastructure.Discord.Services;

namespace DiscordBot.Infrastructure.Discord.Initialization;

/// <summary>
/// Initializes discord modules in <see cref="CommandService"/> from assembly 
/// </summary>
public sealed class CommandsInitialization : InitializationModuleBase
{
    private readonly DiscordSocketClient _client;
    private readonly IServiceProvider _services;
    private readonly CommandService _commandService;
    private readonly CommandHandlingService _commandHandlingService;

    public CommandsInitialization(CommandService commandService,
        IServiceProvider services,
        CommandHandlingService commandHandlingService,
        DiscordSocketClient client)
    {
        _commandService = commandService;
        _services = services;
        _commandHandlingService = commandHandlingService;
        _client = client;
    }
    
    public override ValueTask InitializeAsync()
    {
        _client.MessageReceived += _commandHandlingService.MessageReceivedAsync;
        _commandService.CommandExecuted += _commandHandlingService.CommandExecutedAsync;
        
        _commandService.AddModulesAsync(typeof(CommandsInitialization).Assembly, _services);
        return ValueTask.CompletedTask;
    }
}