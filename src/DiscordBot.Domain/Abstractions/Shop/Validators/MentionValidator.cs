namespace DiscordBot.Domain.Abstractions.Shop.Validators;

public class MentionValidator : IItemValidator
{
    public OperationResult Validate(ShopItem item, BuyOptions options)
    {
        if (options.Mention is null) return OperationResult.FromError("Mention is required");
        return OperationResult.FromSuccess();
    }
}