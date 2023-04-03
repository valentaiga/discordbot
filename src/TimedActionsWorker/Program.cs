using Discord;
using Discord.Rest;
using Discord.WebSocket;
using DiscordBot.Application;
using DiscordBot.Domain.Options;
using DiscordBot.Infrastructure.Redis;
using TimedActionsWorker;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.ConfigureJsonSettings();
        services.ConfigureRedisServices(_ =>
        {
            _.Endpoint = Environment.GetEnvironmentVariable("REDIS_ENDPOINT")!;
            _.Password = Environment.GetEnvironmentVariable("REDIS_PASSWORD")!;
        });

        services.AddSingleton<DiscordRestClient>();
        services.AddSingleton(new DiscordSocketConfig
        {
            AlwaysDownloadUsers = true,
            GatewayIntents = GatewayIntents.All
        });

        services.AddOptions<DiscordClientOptions>()
            .Configure(_ => _.Token = Environment.GetEnvironmentVariable("DISCORD_TOKEN")!)
            .ValidateDataAnnotations();

        services.AddSingleton<ActionHandler>();

        services.AddHostedService<TimedActionsWorker.TimedActionsWorker>();
    })
    .Build();

host.Run();