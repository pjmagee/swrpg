using Orleans;
using Orleans.Runtime;

namespace SWRPG.Game.Server.Grains;

public class TenantsGrain : Grain, IGrainWithIntegerKey
{
    private readonly ILogger<SpeciesGrain> _logger;
    private readonly IPersistentState<List<ulong>> _state;

    public TenantsGrain(ILogger<SpeciesGrain> logger,
        [PersistentState("tenants")] IPersistentState<List<ulong>> state)
    {
        _logger = logger;
        _state = state;
    }
}