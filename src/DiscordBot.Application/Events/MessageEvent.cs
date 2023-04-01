namespace DiscordBot.Application.Events;

public record struct MessageEvent(ulong GuildId, 
    ulong SenderId, 
    string Username, 
    string Nickname, 
    string Text);