using Abilities;

namespace AbilitiesTest.AbilityTests
{
    public class RandomTeleportTest : IAbilityTest
    {
        public string Description => "This will be written later on.";

        public string GetStatsTableTill(int level = 5)
        {
            List<Property> ls = new List<Property>() {
                new Property("Range", new List<string>())
            };

            for (int i = 1; i <= level; i++)
            {
                RandomTeleport ability = new RandomTeleport(i);

                ls[0].Values.Add($"{ability.RangeInBlocks} blocks");
            }

            return Utils.GetPropertiesAsMarkupTable(ls);
        }

        public void Run(int level)
        {
            RandomTeleport ability = new RandomTeleport(level);

            Console.WriteLine($"Range: {ability.RangeInBlocks} blocks");
        }

        public void RunTill(int level = 5)
        {
            for (int i = 1; i <= level; i++)
            {
                Run(i);
            }
        }
    }
}