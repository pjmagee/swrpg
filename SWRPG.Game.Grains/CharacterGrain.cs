using Microsoft.Extensions.Logging;
using Orleans;
using Orleans.Runtime;
using SWRPG.Game.Abstractions.Grains;
using SWRPG.Game.Abstractions.Models;

namespace SWRPG.Game.Grains
{
    public class CharacterGrain : Grain, ICharacterGrain
    {
        private readonly ILogger<CharacterGrain> _logger;

        private readonly IPersistentState<Profile> _profileState;

        private string GrainType => nameof(CharacterGrain);
        private string PrimaryKey => this.GetPrimaryKeyString();

        public CharacterGrain(
            ILogger<CharacterGrain> logger,
            [PersistentState("profile")] IPersistentState<Profile> profileState)
        {
            _logger = logger;
            _profileState = profileState;
        }

        public async Task SetAsync(Profile profile)
        {
            _profileState.State = profile;
            await _profileState.WriteStateAsync();

            _logger.LogInformation(
                "{@GrainType} {@PrimaryKey} now contains {@Profile}", GrainType, PrimaryKey, profile);
        }

        public async Task CreateAsync(ulong userId, ulong guildId, string name)
        {
            _profileState.State = new Profile(userId, guildId, name, DateTimeOffset.Now, 100);
            await _profileState.WriteStateAsync();
        }
    }
}
