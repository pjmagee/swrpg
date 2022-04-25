using Orleans.Concurrency;

namespace SWRPG.Game.Abstractions.Models;


// These can each be implemented as a Command Module

// Pazaak,
// Sabacc, https://starwars.fandom.com/wiki/Sabacc/Legends
// Lugjack machine https://starwars.fandom.com/wiki/Lugjack
// Jubilee wheel https://starwars.fandom.com/wiki/Jubilee_Wheel_(game)
// Pod Racing Betting https://starwars.fandom.com/wiki/Gambling/Legends


[Serializable, Immutable]
public class GamblingSource
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Description { get; }
}