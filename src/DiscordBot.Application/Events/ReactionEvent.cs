namespace DiscordBot.Application.Events;

public record ReactionEvent(ulong GuildId,
    ulong ReactionAuthorId,
    ulong MessageAuthorId,
    ulong MessageId,
    string Reaction);