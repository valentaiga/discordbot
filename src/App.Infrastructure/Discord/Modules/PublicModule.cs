using System.Text;
using Discord;
using Discord.Commands;

namespace App.Infrastructure.Discord.Modules;

[RequireContext(ContextType.Guild)]
public class PublicModule : ModuleBase<SocketCommandContext>
{
    private readonly CommandService _commandService;

    public PublicModule(CommandService commandService)
    {
        _commandService = commandService;
    }
    
    [Command("help")]
    [Summary("Get a list of all commands")]
    [RequireUserPermission(ChannelPermission.SendMessages)]
    public async Task Help()
    {
        await Context.Message.DeleteAsync();

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
}