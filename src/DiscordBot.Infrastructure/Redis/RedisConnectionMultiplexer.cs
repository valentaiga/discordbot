using DiscordBot.Application.Options;
using DiscordBot.Application.Services.Redis;
using Microsoft.Extensions.Options;
using StackExchange.Redis;

namespace DiscordBot.Infrastructure.Redis;

public class RedisConnectionMultiplexer : IRedisConnectionMultiplexer, IDisposable
{
    private readonly ConfigurationOptions _config;
    private readonly SemaphoreSlim _locker = new(1); 
    private ConnectionMultiplexer? _multiplexer;

    public RedisConnectionMultiplexer(IOptions<RedisConfiguration> options)
    {
        _config = new ConfigurationOptions
        {
            Password = options.Value.Password,
            ConnectTimeout = (int)TimeSpan.FromSeconds(2).Ticks, 
            EndPoints =
            {
                options.Value.Endpoint
            }
        };
    }

    public async ValueTask<ISubscriber> GetSubscriberAsync(string channel)
    {
        var multiplexer = await ConnectAsync();
        return multiplexer.GetSubscriber(channel);
    }

    public async ValueTask<IDatabase> GetDatabaseAsync()
    {
        var multiplexer = await ConnectAsync();
        return multiplexer.GetDatabase();
    }

    private async ValueTask<ConnectionMultiplexer> ConnectAsync()
    {
        if (_multiplexer is not null) return _multiplexer;
        await _locker.WaitAsync();
        _multiplexer ??= await ConnectionMultiplexer.ConnectAsync(_config);
        _locker.Release();
        return _multiplexer;
    }

    public void Dispose()
    {
        _multiplexer?.Dispose();
    }
}