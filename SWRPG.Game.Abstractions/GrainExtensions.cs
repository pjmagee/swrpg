using Orleans;

namespace SWRPG.Game.Abstractions;

public static class GrainExtensions
{
    public static T GetGrain<T>(this IGrainFactory grainFactory) where T : IGrainWithSingletonKey
    {
        return grainFactory.GetGrain<T>(Guid.Empty);
    }
}