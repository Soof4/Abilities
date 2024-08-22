using Abilities;

namespace AbilitiesTest.AbilityTests
{
    public class MarthymrTest : IAbilityTest
    {
        public string Description => "This will be written later on.";

        public List<Property> GetStatsAsListTill(int level = 5)
        {
            List<Property> ls = new List<Property>() {
                new Property("Damage", new List<string>()),
                new Property("Projectile Speed", new List<string>())
            };

            for (int i = 1; i <= level; i++)
            {
                Marthymr ability = new Marthymr(i);

                ls[0].Values.Add($"{ability.Damage}");
                ls[1].Values.Add($"{ability.SpeedFactor} mph?");
            }

            return ls;
        }

        public void Run(int level)
        {
            Marthymr ability = new Marthymr(level);

            Console.WriteLine($"Damage: {ability.Damage}");
            Console.WriteLine($"Projectile Speed: {ability.SpeedFactor} mph?");
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