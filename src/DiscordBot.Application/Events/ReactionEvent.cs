namespace DiscordBot.Application.Events;

public record struct ReactionEvent(ulong GuildId,
    ulong ReactionAuthorId,
    ulong MessageAuthorId,
    ulong MessageId,
    string Reaction);