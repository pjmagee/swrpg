using Orleans.Concurrency;

namespace SWRPG.Game.Abstractions.Models;

[Serializable, Immutable]
public class GambleScore
{
    public Guid Id { get; set; }
    public Guid GamblingSourceId { get; set; }
    public Guid CharacterId { get; set; }
    public decimal TotalWon { get; set; }
    public decimal TotalLost { get; set; }
    public int Won { get; set; }
    public int Lost { get; set; }
}