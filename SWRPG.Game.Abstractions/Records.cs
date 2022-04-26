namespace SWRPG.Game.Abstractions;

// These can each be implemented as a Command Module
// Pazaak,
// Sabacc, https://starwars.fandom.com/wiki/Sabacc/Legends
// Lugjack machine https://starwars.fandom.com/wiki/Lugjack
// Jubilee wheel https://starwars.fandom.com/wiki/Jubilee_Wheel_(game)
// Pod Racing Betting https://starwars.fandom.com/wiki/Gambling/Legends

public record GambleSource(string Name, string Description);

public record PlanetInfo(string Name, string? Description);

public record RaceInfo(string Name, string? Description);

public record Profession(string Name, string[] Tags);

public record CharacterProfessionActions(string Action, DateTimeOffset Started, DateTimeOffset? Ended);

public record CharacterProfileInfo(string Name, DateTimeOffset Created, decimal Credits, string Planet);

public record CharacterGambleScore(decimal TotalWon, decimal TotalLost, int GamesWon, int GamesLost);
public record CharacterProfession(string Name, uint Level);
public record CharacterInventory(List<Item> Items);

public record Item(string Name, int Condition, int Quality);

public record LocationInfo(string Name);

public record ProfessionAction(string Action, TimeSpan Min, TimeSpan Mac, float Difficulty);

public record CreatureInfo(string Name, bool IsTameable);