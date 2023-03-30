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
    private ISubscriber? ReactionSubscriber { get; set; }

    public EventPublisher(JsonSerializerOptions serializerOptions)
    {
        _serializerOptions = serializerOptions;
    }

    public void SetMessageSubscriber(ISubscriber sub) => MessageSubscriber = sub;
    public void SetReactionSubscriber(ISubscriber sub) => ReactionSubscriber = sub;

    public async Task PublishMessageAsync(SocketMessage msg)
    {
        if (MessageSubscriber is null) return;
        if (msg.Author.IsBot) return;
        if (string.IsNullOrEmpty(msg.Content)) return;
        if (msg.Channel is not SocketTextChannel channel) return;

        var senderId = msg.Author.Id;
        var guildId = channel.Guild.Id;
        var ev = new MessageEvent(senderId, guildId, msg.Content);

        var json = JsonSerializer.Serialize(ev, _serializerOptions);
        await MessageSubscriber.PublishAsync(
            EventChannels.MessageEventChannel,
            json);
    }

    public async Task PublishReactionAsync(Cacheable<IUserMessage, ulong> cacheMsg,
        Cacheable<IMessageChannel, ulong> cacheChannel,
        SocketReaction reaction)
    {
        if (ReactionSubscriber is null) return;
        if (reaction.User.Value is { IsBot: true }) return;
        
        var msg = await cacheMsg.GetOrDownloadAsync();
        
        if (msg.Author.IsBot) return;
        if (reaction.Channel is not SocketTextChannel channel) return;

        var guildId = channel.Guild.Id;

        var ev = new ReactionEvent(
            guildId,
            reaction.User.Value.Id,
            msg.Author.Id,
            msg.Id,
            reaction.Emote.Name);

        var json = JsonSerializer.Serialize(ev, _serializerOptions);
        await ReactionSubscriber.PublishAsync(
            EventChannels.ReactionEventChannel,
            json);
    }
}