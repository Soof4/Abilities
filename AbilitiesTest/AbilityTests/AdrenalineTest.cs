using Abilities;

namespace AbilitiesTest.AbilityTests
{
    public class AdrenalineTest : IAbilityTest
    {
        public string Description => "This will be written later on.";

        public string GetStatsTableTill(int level = 5)
        {
            List<Property> ls = new List<Property>() {
                new Property("Buffs Duration", new List<string>())
            };

            for (int i = 1; i <= level; i++)
            {
                Adrenaline ability = new Adrenaline(i);

                ls[0].Values.Add($"{ability.BuffDurationInTicks / 60.0:F2}s");

            }

            return Utils.GetPropertiesAsMarkupTable(ls);
        }

        public void Run(int level)
        {
            Adrenaline ab = new Adrenaline(level);

            Console.WriteLine($"Buffs Duration: {ab.BuffDurationInTicks / 60.0:2}s");
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