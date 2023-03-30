using Discord.Commands;
using DiscordBot.Domain.Primitives;
using DiscordBot.Infrastructure.Discord.Services;

namespace DiscordBot.Infrastructure.Discord.Initialization;

/// <summary>
/// Initializes discord modules in <see cref="CommandService"/> from assembly 
/// </summary>
public class CommandsInitialization : InitializationModuleBase
{
    private readonly CommandService _commandService;
    private readonly IServiceProvider _services;
    private readonly CommandHandlingService _commandHandlingService;

    public CommandsInitialization(CommandService commandService, IServiceProvider services, CommandHandlingService commandHandlingService)
    {
        _commandService = commandService;
        _services = services;
        _commandHandlingService = commandHandlingService;
    }
    
    public override ValueTask InitializeAsync()
    {
        _commandService.AddModulesAsync(typeof(CommandsInitialization).Assembly, _services);
        return ValueTask.CompletedTask;
    }
}