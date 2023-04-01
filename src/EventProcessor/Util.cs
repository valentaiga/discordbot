using DiscordBot.Domain.Entities;

namespace EventProcessor;

public static class Util
{
    public static ulong CalculateExperience()
    {
        const int minExp = 15;
        const int maxExp = 25;
        return (ulong)Random.Shared.Next(minExp, maxExp);
    }

    public static void UpdateProfile(UserProfile profile,
        string nickname,
        string? username = null,
        ulong addExperience = 0)
    {
        if (username is not null) profile.Username = username;
        profile.Nickname = nickname;
        profile.Experience += addExperience;
    }
}