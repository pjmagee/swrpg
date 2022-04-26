using System.Collections.Immutable;

using Microsoft.Extensions.Logging;

using Orleans;
using Orleans.Runtime;

using SWRPG.Game.Abstractions;

namespace SWRPG.Game.Grains;

public class SharedGrain : Grain, ISharedGrain
{
    private readonly ILogger<SharedGrain> _logger;
    private readonly IPersistentState<SharedState> _state;

    public SharedGrain(
        ILogger<SharedGrain> logger,
        [PersistentState("shared")] IPersistentState<SharedState> state)
    {
        _logger = logger;
        _state = state;
    }

    public Task<bool> IsNewAsync()
    {
        return Task.FromResult(!_state.RecordExists);
    }

    public Task<SharedState> GetAsync()
    {
        return Task.FromResult(_state.State);
    }

    public async Task SetAsync(SharedState sharedState)
    {
        _state.State = sharedState;
        await _state.WriteStateAsync();
    }
}