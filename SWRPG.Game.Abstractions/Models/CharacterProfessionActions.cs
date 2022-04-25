using Orleans.Concurrency;

namespace SWRPG.Game.Abstractions.Models;


[Serializable, Immutable]
public class CharacterProfessionActions
{
    public Guid Id { get; set; }
    public Guid? ProfessionAction { get; set; }
    public DateTime Started { get; set; }

    public DateTime? Ended { get; set; }
}