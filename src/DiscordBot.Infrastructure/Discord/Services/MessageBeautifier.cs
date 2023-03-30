using Discord;
using DiscordBot.Application;
using DiscordBot.Domain.Entities;

namespace DiscordBot.Infrastructure.Discord.Services;

public class MessageBeautifier
{
    public MessageBeautifier()
    {
        
    }

    public Embed BeautifyProfile(UserProfile profile, IUser dsUser)
    {
        var name = profile.Nickname ?? profile.Username;
        return BuildEmbed(Color.Gold)
            .WithTitle(name)
            .AddField("Karma", $"{profile.Collectors.Karma} {Emojis.CloverEmote}", true)
            .AddField("Cookies", $"{profile.Collectors.Cookies} {Emojis.CookieEmote}", true)
            .AddField("Clowns", $"{profile.Collectors.Clowns} {Emojis.CookieEmote}", true)
            .AddField("Joined server at", profile.JoinedServerAt.ToString("Y"))
            .WithThumbnailUrl(dsUser.GetAvatarUrl(size: 80))
            .Build();
    }

    private static EmbedBuilder BuildEmbed(Color color)
        => new EmbedBuilder().WithColor(color).WithCurrentTimestamp();
}