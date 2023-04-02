using System.Text.Json.Serialization;

namespace DiscordBot.Domain.Entities;

public class GuildSettings
{
    public ulong Id { get; set; }
    public HashSet<ulong> AllowedForCommandsTextChannels { get; set; }

    public GuildSettings(ulong id) : this()
    {
        Id = id;
    }

    [JsonConstructor]
    public GuildSettings()
    {
        AllowedForCommandsTextChannels = new();
    }
}