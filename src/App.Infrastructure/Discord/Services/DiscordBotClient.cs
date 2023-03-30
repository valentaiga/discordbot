using App.Domain.Options;
using App.Domain.Primitives;
using Discord;
using Discord.WebSocket;
using Microsoft.Extensions.Options;

namespace App.Infrastructure.Discord.Services;

public class DiscordBotClient
{
    private readonly DiscordSocketClient _client;
    private readonly IEnumerable<IInitializationModule> _initModules;
    private readonly DiscordClientOptions _settings;

    public DiscordBotClient(DiscordSocketClient client,
        IOptions<DiscordClientOptions> settings,
        IEnumerable<IInitializationModule> initModules)
    {
        _client = client;
        _initModules = initModules;
        _settings = settings.Value;
    }

    public async Task StartAsync()
    {
        await LoginAsync();
        await _client.StartAsync();
        
        foreach (var module in _initModules)
        {
            await module.Init();
        }
    }

    private async Task LoginAsync()
    {
        await _client.LoginAsync(TokenType.Bot, _settings.Token);
    }
}