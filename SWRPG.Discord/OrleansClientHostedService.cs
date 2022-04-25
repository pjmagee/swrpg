using Orleans;

namespace SWRPG.Discord;

public class OrleansClientHostedService : IHostedService, IDisposable
{
    public IClusterClient Client { get; }

    public OrleansClientHostedService(ILoggerProvider loggerProvider)
    {
        Client = new ClientBuilder()
            .UseLocalhostClustering()
            .ConfigureLogging(builder => builder.AddProvider(loggerProvider))
            .Build();
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        await Client.Connect();
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        await Client.Close();
    }

    public void Dispose()
    {
        Client.Dispose();
    }
}