namespace DiscordBot.Domain.Primitives;

public interface IInitializationModule
{
    ValueTask InitAsync();
}