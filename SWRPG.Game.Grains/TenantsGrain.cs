using Microsoft.Extensions.Logging;
using Orleans;
using Orleans.Runtime;

namespace SWRPG.Game.Grains;

public class TenantsGrain : Grain, IGrainWithIntegerKey
{
    private readonly ILogger<SpeciesGrain> _logger;
    private readonly IPersistentState<List<ulong>> _state;

    public TenantsGrain(
        ILogger<SpeciesGrain> logger,
        [PersistentState("tenants", "swrpg")] IPersistentState<List<ulong>> state)
    {
        _logger = logger;
        _state = state;
    }


}