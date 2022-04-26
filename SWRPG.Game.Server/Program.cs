using System.Text.Json;

using Orleans;
using Orleans.Hosting;

using SWRPG.Game.Abstractions;
using SWRPG.Game.Grains;
using SWRPG.Game.Server;
using HostBuilderContext = Microsoft.Extensions.Hosting.HostBuilderContext;

var builder = Host
    .CreateDefaultBuilder(args)
    .UseConsoleLifetime()
    .ConfigureAppConfiguration((context, configurationBuilder) =>
    {
        configurationBuilder.AddJsonFile("appsettings.json");
        configurationBuilder.AddJsonFile($"appsettings.{context.HostingEnvironment.EnvironmentName}.json");
    })
    .UseOrleans((HostBuilderContext ctx, ISiloBuilder builder) =>
    {
        builder.UseLocalhostClustering();

        builder.UseDashboard(options =>
        {
            options.Username = ctx.Configuration.GetValue<string>("Dashboard:Username");
            options.Password = ctx.Configuration.GetValue<string>("Dashboard:Password");
            options.Host = "*";
            options.Port = 8080;
            options.HostSelf = true;
            options.CounterUpdateIntervalMs = 1000;
        });

        builder
            .ConfigureApplicationParts(parts =>
            {
                parts.AddApplicationPart(typeof(SharedGrain).Assembly).WithReferences();
            });

        _ = ctx.Configuration.GetValue<string>("GrainStorage:Strategy") switch
        {
            "InMemory" => builder.AddMemoryGrainStorageAsDefault(),
            "Database" => builder.AddAdoNetGrainStorageAsDefault(options =>
            {
                options.Invariant = "Npgsql";
                options.ConnectionString = ctx.Configuration.GetConnectionString("Default");
                options.UseJsonFormat = true;
                options.IndentJson = true;
            }),
            _ => throw new ArgumentOutOfRangeException()
        };

        builder.AddSimpleMessageStreamProvider("SMS");
        builder.AddMemoryGrainStorage("PubSubStore");

        builder.AddStartupTask<StartupTask>();
    });

using var host = builder.Build();
await host.RunAsync();