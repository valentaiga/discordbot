using DiscordBot.Application.Events;
using DiscordBot.Domain.Abstractions;

namespace EventProcessor.Handlers;

internal sealed class MessageHandler
{
    private readonly IProfileService _profileService;

    public MessageHandler(IProfileService profileService)
    {
        _profileService = profileService;
    }

    public async Task HandleAsync(MessageEvent ev)
    {
        var addExp = Util.CalculateExperience(); 
        await _profileService.UpdateAsync(ev.GuildId, ev.SenderId, profile =>
        {
            Util.UpdateProfile(profile, ev.Nickname, ev.Username, addExp);
        });
    }
}