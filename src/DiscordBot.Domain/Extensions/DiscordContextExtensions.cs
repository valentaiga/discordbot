using System.Diagnostics;

namespace DiscordBot.Domain.Extensions;

public static class DiscordContextExtensions
{
    public static async Task NoThrow(this Task task)
    {
        try
        {
            await task;
        }
        catch (Exception)
        {
            // ignored
        }
    }
}