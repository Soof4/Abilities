using Abilities;

namespace AbilitiesTest.AbilityTests
{
    public class ShockwaveTest : IAbilityTest
    {
        public string Description => "This will be written later on.";

        public string GetStatsTableTill(int level = 5)
        {
            List<Property> ls = new List<Property>() {
                new Property("Base Damage:", new List<string>()),
                new Property("Range:", new List<string>()),
                new Property("Knockback:", new List<string>())

            };

            for (int i = 1; i <= level; i++)
            {
                Shockwave ability = new Shockwave(i);

                ls[0].Values.Add($"{ability.BaseDmg}");
                ls[1].Values.Add($"{ability.Size / 16:F2} blocks");
                ls[2].Values.Add($"{ability.Knockback}");

            }

            return Utils.GetPropertiesAsMarkupTable(ls);
        }

        public void Run(int level)
        {
            Shockwave ability = new Shockwave(level);

            Console.WriteLine($"Base damage: {ability.BaseDmg}");
            Console.WriteLine($"Range: {ability.Size / 16:F2} blocks");
            Console.WriteLine($"Knockback: {ability.Knockback}");
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