using Discord;
using Discord.Commands;
using Discord.WebSocket;
using DiscordBot.Application;
using DiscordBot.Domain.Abstractions;
using DiscordBot.Domain.Options;
using DiscordBot.Domain.Primitives;
using DiscordBot.Infrastructure.Discord.Initialization;
using DiscordBot.Infrastructure.Discord.Services;
using DiscordBot.Infrastructure.Redis;

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

    services.ConfigureJsonSettings();

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