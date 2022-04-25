using Orleans;
using SWRPG.Game.Abstractions.Models;

namespace SWRPG.Game.Abstractions.Grains;

public static class GrainExtensions
{
    public static T GetGrain<T>(this IGrainFactory grainFactory) where T : IGrainWithSingletonKey
    {
        return grainFactory.GetGrain<T>(Guid.Empty);
    }
}

public interface IGrainWithSingletonKey : IGrainWithGuidKey
{
}

public interface ITenantsGrain : IGrainWithSingletonKey
{

}

public record Tenant(ulong Id, ulong Name, DateTimeOffset LastJoined);

public interface ICharacterGrain : IGrainWithGuidCompoundKey
{
    Task SetAsync(Profile profile);

    Task CreateAsync(ulong userId, ulong guildId, string name);
}