using System.Text.Json;
using Discord;
using Discord.WebSocket;

namespace SWRPG.Discord;

public class DiscordBot
{
    private readonly ILogger<DiscordBot> _logger;
    private readonly IConfiguration _configuration;
    private readonly InteractionHandler _interactionHandler;
    private readonly DiscordSocketClient _client;
    private readonly JsonSerializerOptions _options = new() { WriteIndented = true };

    public DiscordBot(
        ILogger<DiscordBot> logger,
        IConfiguration configuration,
        InteractionHandler interactionHandler,
        DiscordSocketClient client)
    {
        _logger = logger;
        _configuration = configuration;
        _interactionHandler = interactionHandler;
        _client = client;
    }

    public async Task StartAsync(CancellationToken cancellationToken = default)
    {
        _client.Log += ClientOnLog;

        await _interactionHandler.InitializeAsync();

        var token = _configuration.GetValue<string>("Settings:Token");

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