using DiscordBot.Application;
using DiscordBot.Domain.Abstractions;
using DiscordBot.Infrastructure.Discord.Services;
using DiscordBot.Infrastructure.Redis;
using EventProcessor.Handlers;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((context, services) =>
    {
        services.ConfigureJsonSettings();
        services.ConfigureRedisServices(_ =>
        {
            _.Endpoint = Environment.GetEnvironmentVariable("REDIS_ENDPOINT")!;
            _.Password = Environment.GetEnvironmentVariable("REDIS_PASSWORD")!;
        });

        services.AddSingleton<IProfileService, ProfileService>();
        
        services.AddSingleton<MessageHandler>();
        services.AddSingleton<ReactionHandler>();
        
        services.AddHostedService<EventProcessor.EventProcessor>();
    })
    .Build();

await host.RunAsync();