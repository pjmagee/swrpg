using Orleans;

namespace SWRPG.Game.Abstractions;

public interface IPlanetGrain : IGrainWithStringKey
{
    Task<PlanetInfo> GetInfoAsync();
    Task ArriveAsync(ICharacterGrain grain);
    Task DepartAsync(ICharacterGrain grain);
}