using System.Text.Json.Serialization;

namespace DiscordBot.Domain.Entities;

public class UserProfile
{
    public ulong Id { get; init; }
    public string? Username { get; set; }
    public string? Nickname { get; set; }
    public Collector Collectors { get; init; } = new();
    public DateTime JoinedServerAt { get; set; }
    
    public UserProfile(ulong userId) => Id = userId;

    [JsonConstructor]
    private UserProfile()
    {
    }
    
    public class Collector
    {
        public int Karma { get; set; }
        public int Clowns { get; set; }
        public int Cookies { get; set; }
    }
}