using Discord;
using Discord.Commands;
using DiscordBot.Domain.Abstractions;

namespace DiscordBot.Domain.Preconditions;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
public class RequiresCommandExecutionGrantAttribute : PreconditionAttribute
{
    public override async Task<PreconditionResult> CheckPermissionsAsync(ICommandContext context, CommandInfo command, IServiceProvider services)
    {
        var guildId = context.Guild.Id;
        var textChannelId = context.Channel.Id;
        var permissionService = services.GetRequiredService<IPermissionService>();
            
        var allowedToExecute =
            await permissionService.IsCommandAllowed(guildId, textChannelId, ChannelPermission.SendMessages);
        if (allowedToExecute)
            return PreconditionResult.FromSuccess();
        
        return PreconditionResult.FromError("Commands are prohibited in channel.");
    }
}