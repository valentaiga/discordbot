using System.Text.Json;
using DiscordBot.Application;
using DiscordBot.Application.Events;
using DiscordBot.Application.Services.Redis;
using StackExchange.Redis;

namespace EventProcessor;

internal class MessageProcessor : BackgroundService
{
    private readonly IRedisConnectionMultiplexer _multiplexer;
    private readonly MessageHandler _messageHandler;
    private readonly JsonSerializerOptions _serializerOptions;

    public MessageProcessor(IRedisConnectionMultiplexer multiplexer, MessageHandler messageHandler, JsonSerializerOptions serializerOptions)
    {
        _multiplexer = multiplexer;
        _messageHandler = messageHandler;
        _serializerOptions = serializerOptions;
    }
    
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var subscriber = await _multiplexer.GetSubscriberAsync(EventChannels.MessageEventChannel);

        var queue = await subscriber.SubscribeAsync(EventChannels.MessageEventChannel);
        
        queue.OnMessage(ProcessNewMessage);
        await Task.Delay(-1, stoppingToken);
        await queue.UnsubscribeAsync();
    }

    private async Task ProcessNewMessage(ChannelMessage message)
    {
        var msgEvent = JsonSerializer.Deserialize<MessageEvent>(message.Message!, _serializerOptions)!;
        await _messageHandler.HandleMessageAsync(msgEvent);
    }
}