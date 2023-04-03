using System.Text.Json;
using Discord.Rest;
using DiscordBot.Application;
using DiscordBot.Application.Messages;
using DiscordBot.Application.Services.Redis;
using DiscordBot.Domain.Abstractions.Shop.Validators;
using StackExchange.Redis;

namespace DiscordBot.Domain.Abstractions.Shop.Items;

public class MuteItem : ShopItem
{
    private readonly SemaphoreSlim _locker = new(1);
    private DiscordRestClient? _client;
    private IRedisConnectionMultiplexer? _multiplexer;
    private ISubscriber? _subscriber;
    private JsonSerializerOptions? _serializerSettings;

    public MuteItem() 
        : base("mute", "Mute a person for 15 minutes", 100, TimeSpan.FromMinutes(15), 300)
    {
    }
    
    protected override IEnumerable<IItemValidator> GetValidators() => new IItemValidator[]
    {
        ItemValidators.BalanceValidation,
        ItemValidators.MentionValidator
    };

    public override async Task ItemActionAsync(ulong guildId, ulong? targetId, IServiceProvider services)
    {
        await InitializeServicesAsync(services);
        var user = await _client!.GetGuildUserAsync(guildId, targetId!.Value);
        await user.ModifyAsync(props => props.Mute = true);

        var action = new DelayedAction(guildId, targetId.Value, DelayedActionType.Unmute, Duration!.Value);
        var json = JsonSerializer.Serialize(action, _serializerSettings);
        await _subscriber!.PublishAsync(RedisChannels.DelayedAction, json);
    }

    private async Task InitializeServicesAsync(IServiceProvider services)
    {
        if (_client is null)
        {
            await _locker.WaitAsync();
            _client ??= services.GetRequiredService<DiscordRestClient>();
            _multiplexer ??= services.GetRequiredService<IRedisConnectionMultiplexer>();
            _subscriber ??= await _multiplexer.GetSubscriberAsync(RedisChannels.DelayedAction);
            _serializerSettings ??= services.GetRequiredService<JsonSerializerOptions>();
            _locker.Release();
        }
    }
}