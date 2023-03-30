using DiscordBot.Application.Options;
using DiscordBot.Application.Services.Redis;

namespace DiscordBot.Infrastructure.Common.Redis;

public static class RedisAppExtensions
{
    public static IServiceCollection ConfigureRedisServices(this IServiceCollection services, Action<RedisConfiguration> configure)
    {
        services.AddSingleton<IRedisProvider, RedisProvider>();
        services.AddSingleton<IRedisConnectionMultiplexer, RedisConnectionMultiplexer>();

        services.AddOptions<RedisConfiguration>()
            .Configure(configure)
            .ValidateDataAnnotations();
        return services;
    }
}