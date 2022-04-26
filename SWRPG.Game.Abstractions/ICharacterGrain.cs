using Orleans;

namespace SWRPG.Game.Abstractions;

public interface ICharacterGrain : IGrainWithIntegerKey
{
    Task SetAsync(CharacterState state);

    Task CreateAsync(CharacterState state);
}