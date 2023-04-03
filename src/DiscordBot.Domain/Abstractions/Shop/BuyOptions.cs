using Discord;

namespace DiscordBot.Domain.Abstractions.Shop;

public sealed class BuyOptions
{
    public int BuyerBalance {get; init; }
    public IUser? Mention {get; init; }
    public string? Input {get; init; }
}