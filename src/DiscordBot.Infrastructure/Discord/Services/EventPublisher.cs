using System.Text.Json;
using Discord;
using Discord.WebSocket;
using DiscordBot.Application;
using DiscordBot.Application.Events;
using StackExchange.Redis;

namespace DiscordBot.Infrastructure.Discord.Services;

public class EventPublisher
{
    private readonly JsonSerializerOptions _serializerOptions;
    private ISubscriber? MessageSubscriber { get; set; }

    public EventPublisher(JsonSerializerOptions serializerOptions)
    {
        _serializerOptions = serializerOptions;
    }

    public void SetMessageSubscriber(ISubscriber sub) => MessageSubscriber = sub;

    public async Task PublishMessageAsync(SocketMessage msg)
    {
        if (MessageSubscriber is null)
            return;
        
        if (msg.Author.IsBot || string.IsNullOrEmpty(msg.Content))
            return;
        
        if (msg.Channel is not SocketTextChannel channel)
            return;

        var senderId = msg.Author.Id;
        var guildId = channel.Guild.Id;
        var ev = new MessageEvent(senderId, guildId, msg.Content);

        var json = JsonSerializer.Serialize(ev, _serializerOptions);
        await MessageSubscriber.PublishAsync(
            EventChannels.MessageEventChannel,
            json);
    }
}