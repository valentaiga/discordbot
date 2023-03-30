using App.Domain.Primitives;
using App.Infrastructure.Discord.Services;
using Discord.Commands;

namespace App.Infrastructure.Discord.Initialization;

/// <summary>
/// Initializes discord modules in <see cref="CommandService"/> from assembly 
/// </summary>
public class CommandsInitialization : IInitializationModule
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
    
    public ValueTask Init()
    {
        _commandService.AddModulesAsync(typeof(CommandsInitialization).Assembly, _services);
        return ValueTask.CompletedTask;
    }
}