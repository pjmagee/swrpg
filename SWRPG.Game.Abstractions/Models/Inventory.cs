using Orleans.Concurrency;

namespace SWRPG.Game.Abstractions.Models;

[Serializable, Immutable]
public class Inventory
{
    public Guid Id { get; set; }
}