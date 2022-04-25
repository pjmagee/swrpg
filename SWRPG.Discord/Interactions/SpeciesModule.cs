using Discord.Interactions;
using Orleans;
using SWRPG.Game.Abstractions.Grains;

namespace SWRPG.Discord.Interactions;

[RequireContext(ContextType.Guild)]
[Group("species", "The species")]
public class SpeciesModule : InteractionModuleBase<SocketInteractionContext>
{
    private readonly IClusterClient _clusterClient;
    private readonly IGrainFactory _grainFactory;

    public SpeciesModule(IClusterClient clusterClient, IGrainFactory grainFactory)
    {
        _clusterClient = clusterClient;
        _grainFactory = grainFactory;
    }

    [SlashCommand("list", "Species that you can choose.")]
    public async Task Create()
    {
        await DeferAsync(true);

        var speciesGrain = _grainFactory.GetGrain<ISpeciesGrain>();

        var species = await speciesGrain.GetSpecies();

        await FollowupAsync(string.Join(", ", species.Select(x => x.Name)));
    }
}