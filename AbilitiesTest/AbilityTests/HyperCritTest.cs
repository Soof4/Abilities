using Abilities;

namespace AbilitiesTest.AbilityTests
{
    public class HyperCritTest : IAbilityTest
    {
        public string Description => "This will be written later on.";

        public List<Property> GetStatsAsListTill(int level = 5)
        {
            List<Property> ls = new List<Property>() {
                new Property("Uses", new List<string>())
            };

            for (int i = 1; i <= level; i++)
            {
                HyperCrit ability = new HyperCrit(i);

                ls[0].Values.Add($"{ability.Uses}");
            }

            return ls;
        }

        public void Run(int level)
        {
            HyperCrit ability = new HyperCrit(level);

            Console.WriteLine($"Damage: {ability.Uses}");
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