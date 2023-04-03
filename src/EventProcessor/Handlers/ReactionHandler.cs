using DiscordBot.Application;
using DiscordBot.Application.Messages;
using DiscordBot.Domain.Abstractions;
using DiscordBot.Domain.Entities;

namespace EventProcessor.Handlers;

internal sealed class ReactionHandler
{
    private readonly IProfileService _profileService;

    private readonly HashSet<string> _trackedReactions = new()
    {
        Emojis.CloverEmote,
        Emojis.ClownEmote,
        Emojis.CookieEmote,
    };

    public ReactionHandler(IProfileService profileService)
    {
        _profileService = profileService;
    }
    
    public async Task HandleAsync(ReactionEvent ev)
    {
        if (!_trackedReactions.Contains(ev.Reaction)) return;
        
        await _profileService.UpdateAsync(ev.GuildId, ev.MessageAuthorId, profile =>
        {
            UpdateCollector(profile, ev.Reaction);
        });
    }

    private static void UpdateCollector(UserProfile profile, string reaction)
    {
        switch (reaction)
        {
            case Emojis.CloverEmote or Emojis.CookieEmote:
                profile.Collectors.Karma++;
                profile.Collectors.Cookies++;
                break;
            case Emojis.ClownEmote:
                profile.Collectors.Karma--;
                profile.Collectors.Clowns++;
                break;
            default: break;
        }
    }
}