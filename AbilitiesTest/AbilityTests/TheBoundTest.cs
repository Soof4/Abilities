using Abilities;

namespace AbilitiesTest.AbilityTests
{
    public class TheBoundTest : IAbilityTest
    {
        public string Description => "This will be written later on.";

        public string GetStatsTableTill(int level = 5)
        {
            List<Property> ls = new List<Property>() {
                new Property("Heal Amount", new List<string>()),
                new Property("Max Distance", new List<string>())
            };

            for (int i = 1; i <= level; i++)
            {
                TheBound ability = new TheBound(i);

                ls[0].Values.Add($"{ability.HealAmount}");
                ls[1].Values.Add($"{ability.MaxDistance / 16:F2} blocks");
            }

            return Utils.GetPropertiesAsMarkupTable(ls);
        }

        public void Run(int level)
        {
            TheBound ability = new TheBound(level);

            Console.WriteLine($"Heal Amount: {ability.HealAmount}");
            Console.WriteLine($"Max Distance: {ability.MaxDistance / 16:F2} blocks");
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