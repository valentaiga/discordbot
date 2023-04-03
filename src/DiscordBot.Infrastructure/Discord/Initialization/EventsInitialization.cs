using Discord.WebSocket;
using DiscordBot.Application;
using DiscordBot.Application.Services.Redis;
using DiscordBot.Domain.Primitives;
using DiscordBot.Infrastructure.Discord.Services;

namespace DiscordBot.Infrastructure.Discord.Initialization;

public sealed class EventsInitialization : InitializationModuleBase
{
    private readonly DiscordSocketClient _client;
    private readonly IRedisConnectionMultiplexer _multiplexer;
    private readonly EventPublisher _eventPublisher;

    public EventsInitialization(DiscordSocketClient client,
        IRedisConnectionMultiplexer multiplexer,
        EventPublisher eventPublisher)
    {
        _client = client;
        _multiplexer = multiplexer;
        _eventPublisher = eventPublisher;
    }
    
    public override async ValueTask InitializeAsync()
    {
        var messageSub = await _multiplexer.GetSubscriberAsync(RedisChannels.MessageEventChannel); 
        _eventPublisher.SetMessageSubscriber(messageSub);
        _client.MessageReceived += _eventPublisher.PublishMessageAsync;

        var reactionSub = await _multiplexer.GetSubscriberAsync(RedisChannels.ReactionEventChannel);
        _eventPublisher.SetReactionSubscriber(reactionSub);
        _client.ReactionAdded += _eventPublisher.PublishReactionAsync;
    }
}