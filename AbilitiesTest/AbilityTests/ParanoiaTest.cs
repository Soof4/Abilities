using Abilities;

namespace AbilitiesTest.AbilityTests
{
    public class ParanoiaTest : IAbilityTest
    {
        public string Description => "This will be written later on.";

        public string GetStatsTableTill(int level = 5)
        {
            List<Property> ls = new List<Property>() {
                new Property("Duration", new List<string>()),
                new Property("Range", new List<string>())
            };

            for (int i = 1; i <= level; i++)
            {
                Paranoia ability = new Paranoia(i);

                ls[0].Values.Add($"{ability.DurationInSecs}s");
                ls[1].Values.Add($"{ability.RangeInBlocks} blocks");
            }

            return Utils.GetPropertiesAsMarkupTable(ls);
        }

        public void Run(int level)
        {
            Paranoia ability = new Paranoia(level);

            Console.WriteLine($"Duration: {ability.DurationInSecs}s");
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