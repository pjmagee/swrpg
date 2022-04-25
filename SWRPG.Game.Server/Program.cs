using Orleans;
using Orleans.Hosting;

using SWRPG.Game.Abstractions.Grains;
using SWRPG.Game.Abstractions.Models;
using SWRPG.Game.Grains;

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
                parts.AddApplicationPart(typeof(TenantsGrain).Assembly).WithReferences();
            });


        builder.AddAdoNetGrainStorageAsDefault(options =>
        {
            options.Invariant = "Npgsql";
            options.ConnectionString = ctx.Configuration.GetConnectionString("Default");
            options.UseJsonFormat = true;
            options.IndentJson = true;
        });

        builder.AddSimpleMessageStreamProvider("SMS");
        builder.AddMemoryGrainStorage("PubSubStore");

        builder.AddStartupTask(async (provider, token) =>
        {
            var grainFactory = provider.GetRequiredService<IGrainFactory>();

            var speciesGrain = grainFactory.GetGrain<ISpeciesGrain>();

            var species = await speciesGrain.GetSpecies();

            if (species.Count == 0)
            {
                await speciesGrain.Add(new Species("Zabrak"));
                await speciesGrain.Add(new Species("Wookiee"));
                await speciesGrain.Add(new Species("Human"));
                await speciesGrain.Add(new Species("Rodian"));
                await speciesGrain.Add(new Species("Bothan"));
                await speciesGrain.Add(new Species("Trandoshan"));
                await speciesGrain.Add(new Species("Mirialans"));
                await speciesGrain.Add(new Species("Twi'lek"));
                await speciesGrain.Add(new Species("Chiss"));
            }
        });
    });

using var host = builder.Build();
await host.RunAsync();