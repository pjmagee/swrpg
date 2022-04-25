using Discord.WebSocket;

namespace SWRPG.Discord.Interactions.Common;

public readonly struct GuildId : IEqualityComparer<GuildId>
{
    public ulong Id { get; }

    public GuildId(SocketGuild guild) : this(guild.Id)
    {

    }

    public GuildId(ulong id)
    {
        Id = id;
    }

    public static GuildId FromGuild(SocketGuild guild) => new(guild);

    public static implicit operator GuildId(SocketGuild guild) => new(guild);

    public static implicit operator GuildId(ulong id) => new(id);

    public bool Equals(GuildId x, GuildId y) => x.Id == y.Id;

    public int GetHashCode(GuildId obj) => obj.Id.GetHashCode();
}