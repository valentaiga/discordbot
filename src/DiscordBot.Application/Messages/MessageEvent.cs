namespace DiscordBot.Application.Messages;

public record struct MessageEvent(ulong GuildId, 
    ulong SenderId, 
    string Username, 
    string Nickname, 
    string Text);