using StackExchange.Redis;

namespace DiscordBot.Application.Services.Redis;

public interface IRedisConnectionMultiplexer
{
    ValueTask<IDatabase> GetDatabaseAsync();
}