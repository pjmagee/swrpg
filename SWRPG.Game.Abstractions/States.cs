namespace SWRPG.Game.Abstractions
{
    public class SharedState
    {
        public SortedDictionary<string, RaceInfo> Races { get; set; } = new();
        public SortedDictionary<string, PlanetInfo> PlanetInfos { get; set; } = new();
        public SortedDictionary<string, Profession> Professions { get; set; } = new();
    }

    public class PlanetState
    {
        public List<LocationInfo> Locations { get; set; } = new();
    }

    public class CharacterState
    {
        public CharacterProfession Profession { get; set; }
        public CharacterGambleScore GambleScore { get; set; }
        public CharacterInventory Inventory { get; set; }
        public CharacterProfileInfo ProfileInfo { get; set; }
    }
}
