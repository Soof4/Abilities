using Abilities;

namespace AbilitiesTest.AbilityTests
{
    public class IceGolemTest : IAbilityTest
    {
        public string Description => "This will be written later on.";

        public List<Property> GetStatsAsListTill(int level = 5)
        {
            List<Property> ls = new List<Property>() {
                new Property("Buffs Duration", new List<string>()),
            };

            for (int i = 1; i <= level; i++)
            {
                IceGolem ability = new IceGolem(i);

                ls[0].Values.Add($"{ability.BuffDurationInTicks / 60.0:F2}s");
            }

            return ls;
        }

        public void Run(int level)
        {
            IceGolem ability = new IceGolem(level);

            Console.WriteLine($"Buffs Duration: {ability.BuffDurationInTicks / 60.0:F2}s");
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