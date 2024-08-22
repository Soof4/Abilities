using Abilities;

namespace AbilitiesTest.AbilityTests
{
    public class HarvestTest : IAbilityTest
    {
        public string Description => "This will be written later on.";

        public List<Property> GetStatsAsListTill(int level = 5)
        {
            List<Property> ls = new List<Property>() {
                new Property("Damage", new List<string>()),
                new Property("Spawn Interval", new List<string>()),
            };

            for (int i = 1; i <= level; i++)
            {
                Harvest ability = new Harvest(i);

                ls[0].Values.Add($"{ability.Damage}");
                ls[1].Values.Add($"{ability.ProjSpawnInterval / 1000.0:F2}s");
            }

            return ls;
        }

        public void Run(int level)
        {
            Harvest ability = new Harvest(level);

            Console.WriteLine($"Damage: {ability.Damage}");
            Console.WriteLine($"Spawn Interval: {ability.ProjSpawnInterval / 1000.0:F2}s");
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