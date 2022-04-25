using Orleans.Concurrency;

namespace SWRPG.Game.Abstractions.Models;


[Serializable, Immutable]
public class CharacterProfession
{
    public Guid Id { get; set; }
    public Guid CharacterId { get; set; }
    public Guid ProfessionId { get; set; }
    public DateTimeOffset Started { get; set; }
    public uint Level { get; set; } // 1 .. 100 +
}