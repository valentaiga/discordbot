namespace DiscordBot.Application.Events;

public sealed record MessageEvent(ulong GuildId, ulong SenderId, string Text);