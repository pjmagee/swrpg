using System.Text.Json;
using Discord;
using Discord.WebSocket;

namespace SWRPG.Discord;

public class DiscordBot
{
    public const long SWG_RPG_SERVER_ID = 967543198262108331;

    private readonly ILogger<DiscordBot> _logger;
    private readonly InteractionHandler _interactionHandler;
    private readonly DiscordSocketClient _client;
    private readonly JsonSerializerOptions _options = new() { WriteIndented = true };

    public DiscordBot(ILogger<DiscordBot> logger, InteractionHandler interactionHandler, DiscordSocketClient client)
    {
        _logger = logger;
        _interactionHandler = interactionHandler;
        _client = client;
    }

    public async Task StartAsync(string token, CancellationToken cancellationToken = default)
    {
        _client.Log += ClientOnLog;

        await _interactionHandler.InitializeAsync();
        await _client.LoginAsync(TokenType.Bot, token, validateToken: true);
        await _client.StartAsync();
    }

    private Task ClientOnLog(LogMessage logMessage)
    {
        _logger.Log(logMessage.GetLogLevel(), JsonSerializer.Serialize(logMessage, _options));

        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.WhenAll(_client.LogoutAsync(), _client.StopAsync());
    }
}