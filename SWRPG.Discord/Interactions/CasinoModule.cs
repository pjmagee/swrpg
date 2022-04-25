using Discord;
using Discord.Interactions;
using SWRPG.Discord.Interactions.Common;

using System.Collections.Concurrent;

namespace SWRPG.Discord.Interactions;

[RequireContext(ContextType.Guild)]
[Group("casino", "Sponsored by the Hutts, ofcourse")]
public class CasinoModule : InteractionModuleBase<SocketInteractionContext>
{
    [RequireContext(ContextType.Guild)]
    [Group("pazak", "Pazak card game")]
    public class PazakModule : InteractionModuleBase<SocketInteractionContext>
    {
        [SlashCommand("start", "start a pazakk game")]
        public async Task Create()
        {

        }

        [SlashCommand("stop", "start a pazakk game")]
        public async Task Stop()
        {

        }
    }

    [RequireContext(ContextType.Guild)]
    [Group("sabacc", "The sabacc card game")]
    public class SabaccModule : InteractionModuleBase<SocketInteractionContext>
    {
        #region Models

        public class CreateSabaccSession : IModal
        {
            public string Title => "Create Sabacc";

            [InputLabel("Ante")]
            [ModalTextInput("ante", initValue: "5.0")]
            public decimal Ante { get; set; }

            [InputLabel("Bombout Penalty")]
            [ModalTextInput("fixed_penalty", initValue: "5.0")]
            public decimal FixedPenalty { get; set; }

            [InputLabel("Minimum bet")]
            [ModalTextInput("min_bet", initValue: "10.0")]
            public decimal MinBet { get; set; }

            [InputLabel("Minimum rounds before Calling hand")]
            [ModalTextInput("min_rounds", initValue: "4")]
            public int MinRoundsBeforeCall { get; set; }
        }

        public static class CustomIds
        {
            public const string Create = "sabacc-table-create";

            public const string Join = "sabacc-table-join";
            public const string Leave = "sabacc-table-leave";

            public const string End = "sabacc-table-end";
            public const string Start = "sabacc-table-start";
        }

        #endregion

        #region Services

        public class SabaccSession
        {
            public IGuild Guild { get; set; }
            public HashSet<IUser> Users { get; set; }

            public DateTimeOffset Created { get; set; }

            public bool ClosedForJoining { get; set; }

            public decimal MainPot { get; set; }
            public decimal SabaccPot { get; set; }
            public decimal MinBetAmount { get; set; }

            public int TotalRounds { get; private set; }
            public int MinRoundsForCall { get; set; }

            public IUser? Current { get; set; }

            public SabaccSession()
            {
                Users = new HashSet<IUser>(8);
                TotalRounds = 0;
                Created = DateTimeOffset.Now;
            }
        }

        public class SabaccSessionManager
        {
            public ConcurrentDictionary<GuildId, SabaccSession> Sessions { get; } = new();

            public SabaccSession GetSession(GuildId id)
            {
                return Sessions[id];
            }

            public bool Create(GuildId id, SabaccSession sabaccSession)
            {
                return Sessions.TryAdd(id, sabaccSession);
            }
        }

        #endregion

        private readonly SabaccSessionManager _sabaccSessionManager;

        public SabaccModule(SabaccSessionManager sabaccSessionManager)
        {
            _sabaccSessionManager = sabaccSessionManager;
        }

        [SlashCommand("create", "create a sabacc game")]
        public async Task Create()
        {
            if (_sabaccSessionManager.Sessions.ContainsKey(Context.Guild))
            {
                await RespondAsync("Sabacc is already in progress", ephemeral: true);
            }
            else
            {
                await RespondWithModalAsync<CreateSabaccSession>(CustomIds.Create);
            }
        }

        [ComponentInteraction(CustomIds.Join, true)]
        public async Task JoinTable()
        {
            if (_sabaccSessionManager.Sessions.TryGetValue(Context.Guild, out var session) && session.Users.Add(Context.User))
            {
                await DeferAsync(false);
                await ModifyOriginalResponseAsync(properties =>
                {
                    properties.Embed = new Optional<Embed>(new EmbedBuilder()
                        .WithTitle("Players")
                        .WithAuthor(Context.Client.CurrentUser)
                        .WithColor(Color.Default)
                        .WithFooter("Waiting for a scum and villainy to join the table")
                        .WithCurrentTimestamp()
                        .WithDescription(string.Join($",\n", session.Users.Select(x => x.Mention)))
                        .Build());
                });
            }
            else
            {
                await RespondAsync("Could not join table.");
            }

        }

        [ComponentInteraction(CustomIds.Leave, true)]
        public async Task LeaveTable()
        {
            if (_sabaccSessionManager.Sessions.TryGetValue(GuildId.FromGuild(Context.Guild), out var session) && session.Users.Remove(Context.User))
            {
                await DeferAsync();
                await ModifyOriginalResponseAsync(properties =>
                {
                    properties.Embed = new Optional<Embed>(new EmbedBuilder()
                        .WithTitle("Players")
                        .WithAuthor(Context.Client.CurrentUser)
                        .WithColor(Color.Default)
                        .WithFooter("Waiting for a scum and villainy to join the table")
                        .WithCurrentTimestamp()
                        .WithDescription(string.Join(",\n", session.Users.Select(x => x.Mention)))
                        .Build());
                });
            }
            else
            {
                await RespondAsync("Could not leave table.");
            }
        }

        [ComponentInteraction(CustomIds.End, true)]
        public async Task EndTable()
        {
            _sabaccSessionManager.Sessions.TryRemove(Context.Guild.Id, out _);
            await DeferAsync(false);
            await DeleteOriginalResponseAsync();
        }

        static class MessageComponents
        {
            public static MessageComponent NewSessionActions { get; }

            static MessageComponents()
            {
                NewSessionActions = new ComponentBuilder()
                    .WithButton("Join", CustomIds.Join, ButtonStyle.Primary)
                    .WithButton("Leave", CustomIds.Leave, ButtonStyle.Primary)
                    .WithButton("Start", CustomIds.Start, ButtonStyle.Success)
                    .WithButton("End", CustomIds.End, ButtonStyle.Danger).Build();
            }

        }

        [ModalInteraction(CustomIds.Create, true)]
        public async Task Create(CreateSabaccSession modal)
        {
            if (!_sabaccSessionManager.Sessions.ContainsKey(Context.Guild))
            {
                var session = new SabaccSession()
                {
                    Guild = Context.Guild,
                    MinBetAmount = modal.MinBet,
                    MinRoundsForCall = modal.MinRoundsBeforeCall,
                    ClosedForJoining = false
                };

                if (_sabaccSessionManager.Create(Context.Guild.Id, session))
                {
                    await RespondAsync(
                        text: "Table now open for joining!",
                        allowedMentions: AllowedMentions.All,
                        components: MessageComponents.NewSessionActions,
                        embed: new EmbedBuilder()
                            .WithTitle("Players")
                            .WithAuthor(Context.Client.CurrentUser)
                            .WithColor(Color.Default)
                            .WithFooter("Waiting for a scum and villainy to join the table")
                            .WithCurrentTimestamp()
                            .WithDescription("Empty")
                            .Build(),
                        ephemeral: false);
                }
            }
        }
    }

}