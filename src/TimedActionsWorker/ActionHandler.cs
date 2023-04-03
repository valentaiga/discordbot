using Discord.Rest;
using DiscordBot.Application;
using DiscordBot.Application.Messages;
using DiscordBot.Domain.Extensions;

namespace TimedActionsWorker;

internal class ActionHandler
{
    private readonly DiscordRestClient _client;

    public ActionHandler(DiscordRestClient client)
    {
        _client = client;
    }

    public Task Handle(DelayedAction action) =>
        action.Action switch
        {
            DelayedActionType.Unmute => Unmute(action),
            _ => Task.CompletedTask
        };

    private async Task Unmute(DelayedAction action)
    {
        var restUser = await _client.GetGuildUserAsync(action.GuildId, action.TargetId);
        await Task.Delay(action.Delay);
        await restUser.ModifyAsync(props => props.Mute = false).NoThrow();
    }
}