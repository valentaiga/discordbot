namespace DiscordBot.Domain.Abstractions.Shop;

public interface IItemValidator
{
    OperationResult Validate(ShopItem item, BuyOptions options);
}