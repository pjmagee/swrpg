using System.Collections.Immutable;

using Orleans;
using Orleans.Runtime;

using SWRPG.Game.Abstractions.Grains;
using SWRPG.Game.Abstractions.Models;

namespace SWRPG.Game.Server.Grains;

public class SpeciesState
{
    public List<Species> Species { get; set; } = new();
}

public class SpeciesGrain : Grain, ISpeciesGrain
{
    private readonly ILogger<SpeciesGrain> _logger;
    private readonly IPersistentState<SpeciesState> _state;

    public SpeciesGrain(ILogger<SpeciesGrain> logger,
        [PersistentState("species")] IPersistentState<SpeciesState> state)
    {
        _logger = logger;
        _state = state;
    }

    public Task<IReadOnlySet<Species>> GetSpecies()
    {
        IReadOnlySet<Species> immutableHashSet = _state.State.Species.ToImmutableHashSet();
        return Task.FromResult(immutableHashSet);
    }

    public async Task<bool> Add(Species species)
    {
        var record = _state.State.Species.Find(x => x.Name.Equals(species.Name, StringComparison.OrdinalIgnoreCase));

        if (record is not null) return false;

        _state.State.Species.Add(species);
        await _state.WriteStateAsync();

        return true;
    }

    public async Task<bool> Remove(string name)
    {
        var record = _state.State.Species.Find(x => x.Name.Equals(name, StringComparison.OrdinalIgnoreCase));

        if (record is not null) _state.State.Species.Remove(record);

        await _state.WriteStateAsync();

        return record is not null;
    }
}