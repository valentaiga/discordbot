using System.Text.Json.Serialization;

namespace DiscordBot.Domain.Entities;

public class UserProfile
{
    public ulong Id { get; set; }
    public string? Username { get; set; }
    public string? Nickname { get; set; }
    public ulong Experience { get; set; }
    public Collector Collectors { get; set; }
    
    public UserProfile(ulong userId)
    {
        Id = userId;
        Collectors = new();
    }

    [JsonConstructor]
    public UserProfile()
    {
        Collectors = new();
    }
    
    public class Collector
    {
        public int Karma { get; set; }
        public int Clowns { get; set; }
        public int Cookies { get; set; }
    }
}