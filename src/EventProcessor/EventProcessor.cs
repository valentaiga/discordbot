using System.Text.Json;
using DiscordBot.Application;
using DiscordBot.Application.Messages;
using DiscordBot.Application.Services.Redis;
using EventProcessor.Handlers;
using StackExchange.Redis;

namespace EventProcessor;

internal class EventProcessor : BackgroundService
{
    private readonly IRedisConnectionMultiplexer _multiplexer;
    private readonly MessageHandler _messageHandler;
    private readonly ReactionHandler _reactionHandler;
    private readonly JsonSerializerOptions _serializerOptions;

    public EventProcessor(IRedisConnectionMultiplexer multiplexer,
        MessageHandler messageHandler,
        ReactionHandler reactionHandler,
        JsonSerializerOptions serializerOptions)
    {
        _multiplexer = multiplexer;
        _messageHandler = messageHandler;
        _reactionHandler = reactionHandler;
        _serializerOptions = serializerOptions;
    }
    
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var messageSubscriber = await _multiplexer.GetSubscriberAsync(RedisChannels.MessageEventChannel);
        var messageQueue = await messageSubscriber.SubscribeAsync(RedisChannels.MessageEventChannel);
        
        var reactionSubscriber = await _multiplexer.GetSubscriberAsync(RedisChannels.ReactionEventChannel);
        var reactionQueue = await reactionSubscriber.SubscribeAsync(RedisChannels.ReactionEventChannel);
        
        messageQueue.OnMessage(ProcessMessage);
        reactionQueue.OnMessage(ProcessReaction);
        
        await Task.Delay(-1, stoppingToken);
        
        await messageQueue.UnsubscribeAsync();
        await reactionQueue.UnsubscribeAsync();
    }

    private async Task ProcessReaction(ChannelMessage message)
    {
        var reactionEvent = Deserialize<ReactionEvent>(message);
        await _reactionHandler.HandleAsync(reactionEvent);
    }

    private async Task ProcessMessage(ChannelMessage message)
    {
        var msgEvent = Deserialize<MessageEvent>(message);
        await _messageHandler.HandleAsync(msgEvent);
    }

    private T Deserialize<T>(ChannelMessage message) =>
        JsonSerializer.Deserialize<T>(message.Message!, _serializerOptions)!;
}