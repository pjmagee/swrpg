using Orleans;
using Orleans.Runtime;
using SWRPG.Game.Abstractions;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace SWRPG.Game.Server
{
    public class StartupTask : IStartupTask
    {
        private readonly IGrainFactory _grainFactory;
        private readonly ILogger<StartupTask> _logger;

        public StartupTask(IGrainFactory grainFactory, ILogger<StartupTask> logger)
        {
            _grainFactory = grainFactory;
            _logger = logger;
        }

        public async Task Execute(CancellationToken cancellationToken)
        {
            var grain = _grainFactory.GetGrain<ISharedGrain>();

            if (await grain.IsNewAsync())
            {


                var planets = JsonSerializer.Deserialize<PlanetInfo[]>(await File.ReadAllTextAsync("Seed/PlanetInfos.json", cancellationToken))!;
                var races = JsonSerializer.Deserialize<RaceInfo[]>(await File.ReadAllTextAsync("Seed/Races.json", cancellationToken))!;

                var state = new SharedState()
                {
                    PlanetInfos = new SortedDictionary<string, PlanetInfo>(planets.ToDictionary(x => x.Name, x => x)),
                    Races = new SortedDictionary<string, RaceInfo>(races.ToDictionary(x => x.Name, x => x)),
                    Professions = new SortedDictionary<string, Profession>()
                };

                await grain.SetAsync(state);

                _logger.LogInformation(JsonSerializer.Serialize(state));
            }
        }
    }
}
