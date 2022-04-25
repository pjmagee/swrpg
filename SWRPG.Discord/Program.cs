using System.Reflection;

using Discord;
using Discord.Interactions;
using Discord.WebSocket;

using MediatR;

using Orleans;

using SWRPG.Discord;
using SWRPG.Discord.Interactions;

using (IHost host = BuildServiceProvider(args))
{
    using (var scope = host.Services.CreateScope())
    {
        var bot = scope.ServiceProvider.GetRequiredService<DiscordBot>();

        await bot.StartAsync(Environment.GetEnvironmentVariable("Token")!, CancellationToken.None);

        await host.RunAsync();
    }
}

public static partial class Program
{
    private static IHost BuildServiceProvider(string[] args) =>
        Host
            .CreateDefaultBuilder(args)
            .ConfigureLogging(loggingBuilder =>
            {
                loggingBuilder.ClearProviders();
                loggingBuilder.AddConsole();
                loggingBuilder.SetMinimumLevel(LogLevel.Trace);
            })
            .ConfigureServices(services =>
            {
                var discordSocketConfig = new DiscordSocketConfig()
                {
                    GatewayIntents = GatewayIntents.AllUnprivileged,
                    AlwaysDownloadUsers = true,
                    LogLevel = LogSeverity.Verbose
                };

                services
                    .AddSingleton<OrleansClientHostedService>()
                    .AddSingleton<IHostedService>(sp => sp.GetService<OrleansClientHostedService>()!)
                    .AddSingleton<IClusterClient>(sp => sp.GetService<OrleansClientHostedService>()!.Client)
                    .AddSingleton<IGrainFactory>(sp => sp.GetService<OrleansClientHostedService>()!.Client);

                services
                    .AddMediatR(Assembly.GetEntryAssembly())
                    .AddSingleton<DiscordBot>()
                    .AddSingleton(provider => new DiscordSocketClient(discordSocketConfig))
                    .AddSingleton<InteractionService>()
                    .AddSingleton<InteractionHandler>()
                    .AddSingleton<CasinoModule.SabaccModule.SabaccSessionManager>();

            })
            .Build();

    public static LogLevel GetLogLevel(this LogMessage message)
    {
        return message.Severity switch
        {
            LogSeverity.Debug => LogLevel.Debug,
            LogSeverity.Critical => LogLevel.Critical,
            LogSeverity.Error => LogLevel.Error,
            LogSeverity.Verbose => LogLevel.Trace,
            LogSeverity.Warning => LogLevel.Warning,
            LogSeverity.Info => LogLevel.Information,
            _ => throw new ArgumentOutOfRangeException()
        };
    }

    public static bool IsDebug()
    {
#if DEBUG
        return true;
#else
                return false;
#endif
    }
}