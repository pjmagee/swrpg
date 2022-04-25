using System.Reflection;
using System.Text.Json;

using Discord;
using Discord.Interactions;
using Discord.Net;
using Discord.WebSocket;

namespace SWRPG.Discord;

public class InteractionHandler
{
    private readonly ILogger<InteractionHandler> _logger;
    private readonly DiscordSocketClient _client;
    private readonly InteractionService _interactions;
    private readonly IServiceProvider _services;

    private JsonSerializerOptions Options = new() { WriteIndented = true };

    public InteractionHandler(
        ILogger<InteractionHandler> logger,
        DiscordSocketClient client,
        InteractionService interactions,
        IServiceProvider services)
    {
        _logger = logger;
        _client = client;
        _interactions = interactions;
        _services = services;
    }

    public async Task InitializeAsync()
    {
        await _interactions.AddModulesAsync(Assembly.GetEntryAssembly(), _services);

        _client.Ready += OnReady;
        _client.InteractionCreated += OnInteractionCreated;
        _client.ModalSubmitted += OnModalSubmitted;

        _interactions.Log += OnInterationsLog;
        _interactions.SlashCommandExecuted += OnSlashCommandExecuted;
        _interactions.ContextCommandExecuted += OnContextCommandExecuted;
        _interactions.ComponentCommandExecuted += OnComponentCommandExecuted;
        _interactions.ModalCommandExecuted += OnModalCommandExecuted;
    }

    private Task OnModalSubmitted(SocketModal arg)
    {
        return Task.CompletedTask;
    }

    private Task OnInterationsLog(LogMessage arg)
    {
        _logger.Log(arg.GetLogLevel(), JsonSerializer.Serialize(new { arg.Message, Exception = arg.Exception.Message, arg.Source }, Options));

        return Task.CompletedTask;
    }

    private async Task OnReady()
    {
        try
        {
            if (Program.IsDebug())
            {
                foreach (var guild in _client.Guilds)
                {
                    await _interactions.RegisterCommandsToGuildAsync(guild.Id);
                }
            }
            else
            {
                await _interactions.RegisterCommandsGloballyAsync();
            }
        }
        catch (HttpException exception)
        {
            _logger.LogError(JsonSerializer.Serialize(exception.Errors, Options));
        }
    }

    private Task OnModalCommandExecuted(ModalCommandInfo arg1, IInteractionContext arg2, IResult arg3)
    {
        return Task.CompletedTask;
    }

    private Task OnComponentCommandExecuted(ComponentCommandInfo arg1, IInteractionContext arg2, IResult arg3)
    {
        return Task.CompletedTask;
    }

    private Task OnContextCommandExecuted(ContextCommandInfo arg1, IInteractionContext arg2, IResult arg3)
    {
        return Task.CompletedTask;
    }

    private Task OnSlashCommandExecuted(SlashCommandInfo arg1, IInteractionContext arg2, IResult arg3)
    {
        return Task.CompletedTask;
    }

    private async Task OnInteractionCreated(SocketInteraction interaction)
    {
        try
        {
            var ctx = new SocketInteractionContext(_client, interaction);

            await _interactions.ExecuteCommandAsync(ctx, _services);
        }
        catch (Exception e)
        {
            _logger.LogError(JsonSerializer.Serialize(e), "execute interaction failed");

            if (interaction.Type == InteractionType.ApplicationCommand)
            {
                var original = await interaction.GetOriginalResponseAsync();
                await original.DeleteAsync();
            }
        }
    }
}