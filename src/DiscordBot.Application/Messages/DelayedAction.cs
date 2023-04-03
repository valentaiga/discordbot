namespace DiscordBot.Application.Messages;

public record struct DelayedAction(ulong GuildId, ulong TargetId, DelayedActionType Action, TimeSpan Delay);