using Discord.Commands;

namespace DiscordBot.Infrastructure.Discord.Modules;

[RequireContext(ContextType.Guild)]
public class ShopModule: ModuleBase<SocketCommandContext>
{
    public ShopModule()
    {
    }

    [Command("shop")]
    [Summary("get a list of available items to buy")]
    public async Task Shop()
    {
        await ReplyAsync("Not implemented yet");
    }
}