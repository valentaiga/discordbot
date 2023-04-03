namespace DiscordBot.Domain.Abstractions.Shop;

public abstract class ShopItem
{
    public string Key { get; }
    public string Description { get; }
    public ushort Price { get; }
    public TimeSpan? Duration { get; }
    public int KarmaRequired { get; }

    protected abstract IEnumerable<IItemValidator> GetValidators();
    public abstract Task ItemActionAsync(ulong guildId, ulong? targetId, IServiceProvider services);

    protected ShopItem(string key, string description, ushort price, TimeSpan? duration, int karmaRequired)
    {
        Key = key;
        Description = description;
        Price = price;
        Duration = duration;
        KarmaRequired = karmaRequired;
    }

    public bool TryValidate(BuyOptions options, out IEnumerable<string> errors)
    {
        var errorList = new List<string>(8);
        foreach (var validator in GetValidators())
        {
            var result = validator.Validate(this, options);
            if (!result.Success) errorList.Add(result.Error!);
        }

        errors = errorList;
        return errorList.Count == 0;
    }
}