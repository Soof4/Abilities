using Abilities;

namespace AbilitiesTest.AbilityTests
{
    public class FairyOfLightTest : IAbilityTest
    {
        public string Description => "Cycles through 4 different sub-abilities:" +
            "\n1: Ethereal Lance   -> Spawns horizontal ethereal lances near the player." +
            "\n2: Dash             -> Dashes towards the direction player is looking at, and creates projectiles behind the player which can damage enemies." +
            "\n3: Prismatic Bolts  -> Periodically spawns prismatic bolts around the player that target the nearby enemies." +
            "\n4: Sundance         -> Webs the player in the location and spawns projectiles that resemble Empress of Light's sundance attack.";

        public List<Property> GetStatsAsListTill(int level = 5)
        {
            List<Property> ls = new List<Property>() {
                new Property("Ethereal Lance Damage", new List<string>()),
                new Property("Dash Damage", new List<string>()),
                new Property("Bolt Damage", new List<string>()),
                new Property("Bolt Spawn Interval", new List<string>()),
                new Property("Sundance Damage", new List<string>()),
            };

            for (int i = 1; i <= level; i++)
            {
                FairyOfLight ability = new FairyOfLight(i);

                ls[0].Values.Add($"{ability.LanceDmg}");
                ls[1].Values.Add($"{ability.DashDmg}");
                ls[2].Values.Add($"{ability.BoltDmg}");
                ls[3].Values.Add($"{ability.BoltSpawnInterval / 1000.0:F2}s");
                ls[4].Values.Add($"{ability.DanceDmg}");
            }

            return ls;
        }

        public void Run(int level)
        {
            FairyOfLight ability = new FairyOfLight(level);

            Console.WriteLine($"Ethereal Lance Damage: {ability.LanceDmg}");
            Console.WriteLine($"Dash Damage: {ability.DashDmg}");
            Console.WriteLine($"Bolt Damage: {ability.BoltDmg}");
            Console.WriteLine($"Bolt Spawn Interval: {ability.BoltSpawnInterval / 60.0:F2}s");
            Console.WriteLine($"Sundance Damage: {ability.DanceDmg}");
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