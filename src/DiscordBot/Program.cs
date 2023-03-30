using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using DiscordBot.Domain.Abstractions;
using DiscordBot.Domain.Options;
using DiscordBot.Domain.Primitives;
using DiscordBot.Infrastructure.Common.Redis;
using DiscordBot.Infrastructure.Discord.Initialization;
using DiscordBot.Infrastructure.Discord.Services;

var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(ConfigureServices)
    .Build();
var dsClient = host.Services.GetRequiredService<DiscordBotClient>();
await dsClient.StartAsync();

await host.StartAsync();
await host.WaitForShutdownAsync();

void ConfigureServices(IServiceCollection services)
{
    services
        .AddSingleton(new DiscordSocketConfig
        {
            AlwaysDownloadUsers = true,
            MessageCacheSize = 100,
            GatewayIntents = GatewayIntents.All
        })
        .AddSingleton<DiscordSocketClient>()
        .AddSingleton<CommandService>()
        .AddSingleton<CommandHandlingService>()
        .AddSingleton<DiscordBotClient>();

    services.AddSingleton<EventPublisher>();
    
    services.AddSingleton<IProfileService, ProfileService>();
    services.AddSingleton<MessageBeautifier>();

    services.AddSingleton(_ => new JsonSerializerOptions()
    {
        WriteIndented = false,
        Encoder = JavaScriptEncoder.Create(UnicodeRanges.Cyrillic, UnicodeRanges.BasicLatin)
    });

    services.AddSingleton<IInitializationModule, CommandsInitialization>();
    services.AddSingleton<IInitializationModule, EventsInitialization>();

    services.AddOptions<DiscordClientOptions>()
        .Configure(_ => _.Token = Environment.GetEnvironmentVariable("DISCORD_TOKEN")!)
        .ValidateDataAnnotations();

    services.ConfigureRedisServices(_ =>
    {
        _.Endpoint = Environment.GetEnvironmentVariable("REDIS_ENDPOINT")!;
        _.Password = Environment.GetEnvironmentVariable("REDIS_PASSWORD")!;
    });
}