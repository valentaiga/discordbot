using DiscordBot.Application.Events;
using DiscordBot.Domain.Abstractions;

namespace EventProcessor;

internal sealed class MessageHandler
{
    private readonly IProfileService _profileService;

    public MessageHandler(IProfileService profileService)
    {
        _profileService = profileService;
    }

    public async Task HandleMessageAsync(MessageEvent msg)
    {
        var addExp = Util.CalculateExperience(); 
        await _profileService.UpdateAsync(msg.GuildId, msg.SenderId, profile =>
        {
            Util.UpdateProfile(profile, msg.Nickname, msg.Username, addExp);
        });
    }
}