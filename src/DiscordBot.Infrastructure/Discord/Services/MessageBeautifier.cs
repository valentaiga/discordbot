using Discord;
using Discord.WebSocket;
using DiscordBot.Application;
using DiscordBot.Domain.Entities;

namespace DiscordBot.Infrastructure.Discord.Services;

public class MessageBeautifier
{
    private readonly Color _defaultColor;

    public MessageBeautifier()
    {
        _defaultColor = Color.Purple;
    }

    public Embed BeautifyProfile(UserProfile profile,
        SocketGuildUser dsUser,
        string? executedCommand = null)
    {
        var name = profile.Nickname ?? profile.Username;
        return BuildEmbed(_defaultColor, executedCommand)
            .WithTitle(name)
            .AddField("Cookies", $"{profile.Collectors.Cookies} {Emojis.CookieEmote}", true)
            .AddField("Karma", $"{profile.Collectors.Karma} {Emojis.CloverEmote}", true)
            .AddField("Clowns", $"{profile.Collectors.Clowns} {Emojis.ClownEmote}", true)
            .AddField("Joined server at", dsUser.JoinedAt!.Value.ToString("Y"))
            .WithThumbnailUrl(dsUser.GetAvatarUrl(size: 80))
            .Build();
    }

    public Embed BuildEmbedLine(string? executedCommand)
    {
        return BuildEmbed(_defaultColor, executedCommand).Build();
    }

    private static EmbedBuilder BuildEmbed(Color color, string? executedCommand)
    {
        var embed = new EmbedBuilder().WithColor(color).WithCurrentTimestamp();
        if (executedCommand is not null) embed.WithFooter(executedCommand);
        return embed;
    }
}