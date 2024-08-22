using Abilities;

namespace AbilitiesTest.AbilityTests
{
    public class RingOfDraculaTest : IAbilityTest
    {
        public string Description => "Summons a circle around the player that chooses the enemy with highest hp and steals some health from it.";

        public List<Property> GetStatsAsListTill(int level = 5)
        {
            List<Property> ls = new List<Property>() {
                new Property("Range", new List<string>())
            };

            for (int i = 1; i <= level; i++)
            {
                RingOfDracula ability = new RingOfDracula(i);

                ls[0].Values.Add($"{ability.RangeInBlocks} blocks");
            }

            return ls;
        }

        public void Run(int level)
        {
            RingOfDracula ability = new RingOfDracula(level);

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