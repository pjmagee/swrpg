using Orleans.Concurrency;

namespace SWRPG.Game.Abstractions.Models;

[Serializable, Immutable]
public class Species
{
    public string Name { get; set; }

    public Species(string name)
    {
        Name = name;
    }
}