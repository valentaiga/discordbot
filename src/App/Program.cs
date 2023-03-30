using App.Domain.Options;
using App.Domain.Primitives;
using App.Infrastructure.Discord.Initialization;
using App.Infrastructure.Discord.Services;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.AspNetCore;

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

    services.AddSingleton<IInitializationModule, CommandsInitialization>();

    services.AddOptions<DiscordClientOptions>()
        .Configure(_ => _.Token = Environment.GetEnvironmentVariable("DISCORD_TOKEN")!)
        .ValidateDataAnnotations();
}