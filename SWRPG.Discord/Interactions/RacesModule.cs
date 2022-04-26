using Discord.Interactions;
using Orleans;
using SWRPG.Game.Abstractions;

namespace SWRPG.Discord.Interactions;

[RequireContext(ContextType.Guild)]
[Group("races", "The races")]
public class RacesModule : InteractionModuleBase<SocketInteractionContext>
{
    private readonly IClusterClient _clusterClient;
    private readonly IGrainFactory _grainFactory;

    public RacesModule(IClusterClient clusterClient, IGrainFactory grainFactory)
    {
        _clusterClient = clusterClient;
        _grainFactory = grainFactory;
    }

    [SlashCommand("list", "Races that you can choose.")]
    public async Task Create()
    {
        await DeferAsync(true);

        var sharedGrain = _grainFactory.GetGrain<ISharedGrain>();

        var shared = await sharedGrain.GetAsync();

        await FollowupAsync(string.Join(", ", shared.Races.Keys));
    }
}