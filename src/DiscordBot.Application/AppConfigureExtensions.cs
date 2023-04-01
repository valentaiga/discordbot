using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;

namespace DiscordBot.Application;

public static class AppConfigureExtensions
{
    public static IServiceCollection ConfigureJsonSettings(this IServiceCollection services)
    {
        services.AddSingleton(_ => new JsonSerializerOptions()
        {
            WriteIndented = false,
            IncludeFields = true,
            Encoder = JavaScriptEncoder.Create(UnicodeRanges.Cyrillic, UnicodeRanges.BasicLatin)
        });
        return services;
    }
}