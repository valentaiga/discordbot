using Discord;
using Discord.Rest;
using Discord.WebSocket;
using DiscordBot.Domain.Options;
using DiscordBot.Domain.Primitives;
using Microsoft.Extensions.Options;

namespace DiscordBot.Infrastructure.Discord.Services;

public class DiscordBotClient : IAsyncDisposable
{
    private readonly DiscordSocketClient _socketClient;
    private readonly DiscordRestClient _restClient;
    private readonly IEnumerable<IInitializationModule> _initModules;
    private readonly DiscordClientOptions _settings;

    public DiscordBotClient(DiscordSocketClient socketClient,
        DiscordRestClient restClient,
        IOptions<DiscordClientOptions> settings,
        IEnumerable<IInitializationModule> initModules)
    {
        _socketClient = socketClient;
        _restClient = restClient;
        _initModules = initModules;
        _settings = settings.Value;
    }

    public async Task StartAsync()
    {
        await LoginAsync();
        await _socketClient.StartAsync();

        foreach (var module in _initModules)
        {
            await module.InitAsync();
        }
    }

    private async Task LoginAsync()
    {
        await _socketClient.LoginAsync(TokenType.Bot, _settings.Token);
        await _restClient.LoginAsync(TokenType.Bot, _settings.Token);
    }

    public async ValueTask DisposeAsync()
    {
        await _socketClient.StopAsync();
        await _restClient.LogoutAsync();
        await _socketClient.LogoutAsync();
    }
}