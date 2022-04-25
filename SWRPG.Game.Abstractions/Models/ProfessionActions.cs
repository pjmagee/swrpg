using Orleans.Concurrency;

namespace SWRPG.Game.Abstractions.Models;

[Serializable, Immutable]
public class ProfessionActions
{
    public Guid Id { get; set; }
    public string Name { get; set; } // Smuggle goods, Jedi training, Bounty for hire, Espionage, Defend the line, Explore sith temple
    public TimeSpan MinDuration { get; set; } // 1 day
    public TimeSpan MaxDuration { get; set; } // 3 days
    public int Difficulty { get; set; } // Some tasks should have a higher chance of failing?
}