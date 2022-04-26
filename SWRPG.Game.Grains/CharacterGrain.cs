using Microsoft.Extensions.Logging;

using Orleans;
using Orleans.Runtime;

using SWRPG.Game.Abstractions;

namespace SWRPG.Game.Grains
{
    public class CharacterGrain : Grain, ICharacterGrain
    {
        private readonly ILogger<CharacterGrain> _logger;
        private readonly IPersistentState<CharacterState> _characterState;
        private readonly IGrainFactory _factory;

        private IPlanetGrain? _planet;

        public CharacterGrain(
            ILogger<CharacterGrain> logger,
            [PersistentState("character")] IPersistentState<CharacterState> characterState,
            IGrainFactory factory)
        {
            _logger = logger;
            _characterState = characterState;
            _factory = factory;
        }

        public override Task OnActivateAsync()
        {
            if (_characterState.RecordExists)
            {
                // The character has been created already
            }
            else
            {
                // This is a new character
            }

            // SetAsync the Worker
            RegisterTimer(Work, null, TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(10));

            return base.OnActivateAsync();
        }

        private async Task Work(object arg)
        {
            if (_planet == null)
            {
                var sharedGrain = _factory.GetGrain<SharedGrain>();
                var sharedState = await sharedGrain.GetAsync();
                var planetInfo = sharedState.PlanetInfos.OrderBy(x => Guid.NewGuid()).First()!;

                _planet = _factory.GetGrain<IPlanetGrain>(planetInfo.Value.Name);
                await _planet.ArriveAsync(this);
            }
            else
            {
                await _planet.DepartAsync(this);
            }

            await Task.CompletedTask;
        }

        public async Task SetAsync(CharacterState state)
        {
            _characterState.State = state;
            await _characterState.WriteStateAsync();
        }

        public async Task CreateAsync(CharacterState state)
        {
            _characterState.State = state;
            await _characterState.WriteStateAsync();
        }
    }
}
