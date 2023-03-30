using Discord;
using Discord.WebSocket;
using DiscordBot.Domain.Options;
using DiscordBot.Domain.Primitives;
using Microsoft.Extensions.Options;

namespace DiscordBot.Infrastructure.Discord.Services;

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
            await module.InitAsync();
        }
    }

    private async Task LoginAsync()
    {
        await _client.LoginAsync(TokenType.Bot, _settings.Token);
    }
}