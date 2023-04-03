namespace DiscordBot.Domain.Abstractions.Shop.Validators;

public class BalanceValidation : IItemValidator
{
    public OperationResult Validate(ShopItem item, BuyOptions options)
    {
        if (options.BuyerBalance < item.Price) return OperationResult.FromError("Your cookie balance is to low.");
        return OperationResult.FromSuccess();
    }
}