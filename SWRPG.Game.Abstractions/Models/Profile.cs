using Orleans.Concurrency;

namespace SWRPG.Game.Abstractions.Models;

[Serializable, Immutable]
public record Profile(
    ulong GuildId,
    ulong UserId,
    string Name,
    DateTimeOffset Created,
    decimal Credits);

