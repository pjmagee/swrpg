using Discord.Interactions;
using SWRPG.Discord.Interactions.Common;

namespace SWRPG.Discord.Interactions;

[RequireContext(ContextType.Guild)]
[Group("character", "Create or modify your character")]
public class CharacterModule : OrleansInteractionModuleBase<SocketInteractionContext>
{
    [RequireContext(ContextType.Guild)]
    [SlashCommand("create", "Create your character")]
    public Task Create(
        [Summary(description: "name")] string name,
        [Summary(description: "species")] string speciesId
    )
    {
        return RespondAsync("Not implemented.");
    }

    [RequireContext(ContextType.Guild)]
    [Group("profession", "manage your character professions")]

    public class ProfessionModule : OrleansInteractionModuleBase<SocketInteractionContext>
    {
        [SlashCommand("add", "Pick a profession to add it to your character")]
        public async Task Pick([Summary(description: "The profession you want to choose")] string professionId)
        {
            await DeferAsync(true);

            // TODO: AddRace this profession to the users professions

            await FollowupAsync("Done.");
        }

        [SlashCommand("list", "list your character professions")]
        public async Task List()
        {
            await DeferAsync(true);

            // TODO: AddRace this profession to the users professions

            await FollowupAsync("Done.");
        }
    }
}