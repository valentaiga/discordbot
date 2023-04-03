using System.Text.Json;
using Discord;
using Discord.Rest;
using DiscordBot.Application;
using DiscordBot.Application.Messages;
using DiscordBot.Application.Services.Redis;
using DiscordBot.Domain.Options;
using DiscordBot.Infrastructure.Redis;
using Microsoft.Extensions.Options;
using StackExchange.Redis;

namespace TimedActionsWorker;

internal class TimedActionsWorker : BackgroundService
{
    private readonly DiscordRestClient _client;
    private readonly DiscordClientOptions _discordOptions;
    private readonly ActionHandler _actionHandler;
    private readonly IRedisConnectionMultiplexer _multiplexer;
    private readonly JsonSerializerOptions _serializerOptions;

    public TimedActionsWorker(DiscordRestClient client,
        IOptions<DiscordClientOptions> discordOptions,
        ActionHandler actionHandler,
        IRedisConnectionMultiplexer multiplexer,
        JsonSerializerOptions serializerOptions)
    {
        _client = client;
        _discordOptions = discordOptions.Value;
        _actionHandler = actionHandler;
        _multiplexer = multiplexer;
        _serializerOptions = serializerOptions;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await _client.LoginAsync(TokenType.Bot, _discordOptions.Token);
        var sub = await _multiplexer.GetSubscriberAsync(RedisChannels.DelayedAction);
        var queue = await sub.SubscribeAsync(RedisChannels.DelayedAction);
        queue.OnMessage(OnDelayedMessage);
        
        await Task.Delay(-1, stoppingToken);
        
        await queue.UnsubscribeAsync();
    }

    private Task OnDelayedMessage(ChannelMessage channelMessage)
    {
        var msg = JsonSerializer.Deserialize<DelayedAction>(channelMessage.Message!, _serializerOptions);
        return _actionHandler.Handle(msg);
    }
}