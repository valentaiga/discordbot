using System.Text;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using DiscordBot.Domain.Abstractions;
using DiscordBot.Domain.Preconditions;
using DiscordBot.Infrastructure.Discord.Services;

namespace DiscordBot.Infrastructure.Discord.Modules;

[RequireContext(ContextType.Guild)]
[RequiresCommandExecutionGrant]
public class PublicModule : ModuleBase<SocketCommandContext>
{
    private readonly CommandService _commandService;
    private readonly IProfileService _profileService;
    private readonly MessageBeautifier _beautifier;

    public PublicModule(CommandService commandService, IProfileService profileService, MessageBeautifier beautifier)
    {
        _commandService = commandService;
        _profileService = profileService;
        _beautifier = beautifier;
    }
    
    [Command("help")]
    [Summary("get a list of all commands")]
    public async Task Help()
    {
        var msg = new StringBuilder();
        msg.AppendLine("```js");
    
        foreach (var module in _commandService.Modules)
        {
            msg.AppendLine(module.Name);
    
            foreach (var command in module.Commands) 
                msg.AppendLine($"  {command.Name}: {command.Summary}");
        }
        
        msg.AppendLine("```");
    
        await ReplyAsync(msg.ToString());
    }

    [Command("profile")]
    [Summary("get profile")]
    public async Task Profile(IUser? user = null)
    {
        var guildId = Context.Guild.Id;
        var dsUser = (SocketGuildUser)(user ?? Context.User);
        var profile = await _profileService.GetAsync(guildId, dsUser.Id);
        
        var msg = _beautifier.BeautifyProfile(profile, dsUser, Context.Message.Content);
    
        await ReplyAsync(embed: msg);
    }
}