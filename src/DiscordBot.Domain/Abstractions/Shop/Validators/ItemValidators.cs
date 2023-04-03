namespace DiscordBot.Domain.Abstractions.Shop.Validators;

public static class ItemValidators
{
    public static readonly BalanceValidation BalanceValidation = new();
    public static readonly MentionValidator MentionValidator = new();
}