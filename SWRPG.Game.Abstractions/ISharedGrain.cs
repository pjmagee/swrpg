using System.Collections.Immutable;

namespace SWRPG.Game.Abstractions;

public interface ISharedGrain : IGrainWithSingletonKey
{
    Task<bool> IsNewAsync();

    Task<SharedState> GetAsync();
    Task SetAsync(SharedState sharedState);
}