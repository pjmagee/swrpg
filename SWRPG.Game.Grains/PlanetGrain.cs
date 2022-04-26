using Orleans;
using Orleans.Runtime;

using SWRPG.Game.Abstractions;

namespace SWRPG.Game.Grains;

public class PlanetGrain : Grain, IPlanetGrain, IGrainWithStringKey
{
    private readonly IPersistentState<PlanetState> _state;
    private readonly IGrainFactory _grainFactory;
    private List<ICharacterGrain> _characterGrains = new();

    public PlanetGrain(
        [PersistentState("planet")] IPersistentState<PlanetState> state,
        IGrainFactory grainFactory)
    {
        _state = state;
        _grainFactory = grainFactory;
    }

    public override Task OnActivateAsync()
    {
        return base.OnActivateAsync();
    }

    public async Task<PlanetInfo> GetInfoAsync()
    {
        var sharedGrain = _grainFactory.GetGrain<ISharedGrain>();
        var sharedState = await sharedGrain.GetAsync();
        return sharedState.PlanetInfos[this.GetPrimaryKeyString()];
    }

    public Task ArriveAsync(ICharacterGrain grain)
    {
        _characterGrains.Add(grain);
        return Task.CompletedTask;
    }

    public Task DepartAsync(ICharacterGrain grain)
    {
        _characterGrains.Remove(grain);
        return Task.CompletedTask;
    }
}