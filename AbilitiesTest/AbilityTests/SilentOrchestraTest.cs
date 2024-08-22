using Abilities;

namespace AbilitiesTest.AbilityTests
{
    public class SilentOrchestraTest : IAbilityTest
    {
        public string Description => "This will be written later on.";

        public List<Property> GetStatsAsListTill(int level = 5)
        {
            List<Property> ls = new List<Property>() {
                new Property("Base Damage", new List<string>()),
                new Property("Base Ability Duration", new List<string>())

            };

            for (int i = 1; i <= level; i++)
            {
                SilentOrchestra ability = new SilentOrchestra(i);

                ls[0].Values.Add($"{ability.BaseDamage}");
                ls[1].Values.Add("20s");

            }

            return ls;
        }

        public void Run(int level)
        {
            SilentOrchestra ability = new SilentOrchestra(level);

            Console.WriteLine($"Base Damage: {ability.BaseDamage}");
            Console.WriteLine("Base Ability Duration: 20s");
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