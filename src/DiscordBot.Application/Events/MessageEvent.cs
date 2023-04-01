namespace DiscordBot.Application.Events;

public sealed record MessageEvent(ulong GuildId, ulong SenderId, string Username, string Nickname, string Text);