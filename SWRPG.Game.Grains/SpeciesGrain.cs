using System.Collections.Immutable;

using Microsoft.Extensions.Logging;

using Orleans;
using Orleans.Runtime;

using SWRPG.Game.Abstractions.Grains;
using SWRPG.Game.Abstractions.Models;

namespace SWRPG.Game.Grains;

public class SpeciesGrain : Grain, ISpeciesGrain
{
    private readonly ILogger<SpeciesGrain> _logger;
    private readonly IPersistentState<HashSet<Species>> _state;

    public SpeciesGrain(
        ILogger<SpeciesGrain> logger,
        [PersistentState("")] IPersistentState<HashSet<Species>> state)
    {
        _logger = logger;
        _state = state;
    }

    public Task<IReadOnlySet<Species>> GetSpecies()
    {
        IReadOnlySet<Species> immutableHashSet = _state.State.ToImmutableHashSet();
        return Task.FromResult(immutableHashSet);
    }

    public async Task<bool> Add(Species species)
    {
        if (_state.State.Add(species))
        {
            await _state.WriteStateAsync();
            return true;
        }

        return false;
    }

    public async Task<bool> Remove(string name)
    {
        var count = _state.State.RemoveWhere(x => x.Name.Equals(name, StringComparison.OrdinalIgnoreCase));

        var removed = count > 0;

        if (removed)
            await _state.WriteStateAsync();

        return removed;

    }
}