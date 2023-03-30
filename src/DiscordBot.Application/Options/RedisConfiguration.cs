namespace DiscordBot.Application.Options;

public class RedisConfiguration
{
    public required string Endpoint { get; set; } = null!;
    public required string Password { get; set; } = null!;
}