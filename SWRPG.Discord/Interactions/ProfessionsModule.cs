using Discord.Interactions;

namespace SWRPG.Discord.Interactions;

[Group("professions", "Professions for your character")]
public class ProfessionsModule : InteractionModuleBase<SocketInteractionContext>
{
    [SlashCommand("list", "List the available professions that you can pick")]
    public async Task Create()
    {
        // TODO: list professions to the user
    }
}