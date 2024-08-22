using Abilities;

namespace AbilitiesTest.AbilityTests
{
    public class PentagramTest : IAbilityTest
    {
        public string Description => "This will be written later on.";

        public List<Property> GetStatsAsListTill(int level = 5)
        {
            List<Property> ls = new List<Property>() {
                new Property("Range", new List<string>())
            };

            for (int i = 1; i <= level; i++)
            {
                Pentagram ability = new Pentagram(i);

                ls[0].Values.Add($"{ability.RangeInBlocks} blocks");
            }

            return ls;
        }

        public void Run(int level)
        {
            Pentagram ability = new Pentagram(level);

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