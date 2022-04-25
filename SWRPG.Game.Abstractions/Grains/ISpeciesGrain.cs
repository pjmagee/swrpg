using Orleans;
using SWRPG.Game.Abstractions.Models;

namespace SWRPG.Game.Abstractions.Grains;

public interface ISpeciesGrain : IGrainWithSingletonKey
{
    Task<IReadOnlySet<Species>> GetSpecies();

    Task<bool> Add(Species species);

    Task<bool> Remove(string name);
}