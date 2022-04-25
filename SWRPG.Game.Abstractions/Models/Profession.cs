using Orleans.Concurrency;

namespace SWRPG.Game.Abstractions.Models;

[Serializable, Immutable]
public class Profession
{
    public Guid Id { get; set; }

    // For discord commands
    public string Tag { get; set; }

    // We want the curve to become steeper the higher their level in the profession
    // So that each time they level, it will take longer to get to the next level
    public float ExpModifier { get; set; }

    // Entertainer, Bounty Hunter, Jedi, Sith, Smuggler, Doctor, Trooper, Agent, Politican
    public string FriendlyName { get; set; }
}