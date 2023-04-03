using Discord;
using Discord.Commands;
using DiscordBot.Application;
using DiscordBot.Domain.Abstractions;
using DiscordBot.Domain.Abstractions.Shop;
using DiscordBot.Domain.Abstractions.Shop.Items;
using DiscordBot.Infrastructure.Discord.Services;

namespace DiscordBot.Infrastructure.Discord.Modules;

[RequireContext(ContextType.Guild)]
public class ShopModule: ModuleBase<SocketCommandContext>
{
    private readonly IServiceProvider _services;
    private readonly IProfileService _profileService;
    private readonly MessageBeautifier _messageBeautifier;

    private readonly List<ShopItem> _items = new()
    {
        new MuteItem(), 

    };
    
    public ShopModule(IServiceProvider services)
    {
        _services = services;
        _profileService = services.GetRequiredService<IProfileService>();
        _messageBeautifier = services.GetRequiredService<MessageBeautifier>();
    }

    [Command("shop")]
    [Summary("get a list of available items to buy")]
    public async Task Shop()
    {
        var embed = _messageBeautifier.BuildEmbedLine("shop");
        embed.WithTitle("Available items for purchase");
        
        foreach (var shopItem in _items)
        {
            embed.AddField($"[{shopItem.Key} - {shopItem.Price}{Emojis.CookieEmote}]", shopItem.Description);
        }
        
        await ReplyAsync(embed: embed.Build());
    }

    [Command("buy")]
    [Summary("buy an item from a list")]
    public async Task Buy(string? input = null, IUser? mention = null)
    {
        var itemKey = input;
        var item = _items.Find(_ => _.Key == itemKey);
        if (item is null)
        {
            await ReplyAsync($"Item [{itemKey}] not found.");
            return;
        }

        var profile = await _profileService.GetAsync(Context.Guild.Id, Context.User.Id);
        var options = new BuyOptions()
        {
            Input = input,
            BuyerBalance = profile.Collectors.Cookies,
            Mention = mention
        };
        if (!item.TryValidate(options, out var errors))
        {
            await ReplyAsync(errors.First());
            return;
        }

        await _profileService.UpdateAsync(Context.Guild.Id, Context.User.Id,
            _ => _.Collectors.Cookies -= item.Price);
        await item.ItemActionAsync(Context.Guild.Id, mention.Id, _services);
        await ReplyAsync($"Item {itemKey} successfully purchased by {Context.User.Mention}!");
    }
}