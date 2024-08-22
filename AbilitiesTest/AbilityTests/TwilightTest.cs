using Abilities;

namespace AbilitiesTest.AbilityTests
{
    public class TwilightTest : IAbilityTest
    {
        public string Description => "Cycles through 3 different sub-abilities:" +
            "\n1: Brilliant Eyes   -> Get vision buffs and spawn projectiles around the player that target the nearby enemies." +
            "\n2: Judgement        -> Dazes the player for a bit and attacks enemies within a range. If an enemy dies after the attack, they'll spawn a projectile which will attack other enemies." +
            "\n3: Punishment       -> Creates an explosion near the player that damages enemies nearby and buffs the player for a few seconds.";

        public List<Property> GetStatsAsListTill(int level = 5)
        {
            List<Property> ls = new List<Property>() {
                new Property("Brilliant Eyes Damage", new List<string>()),
                new Property("Brilliant Eyes Buffs Duration", new List<string>()),
                new Property("Brilliant Eyes Count", new List<string>()),
                new Property("Judgement Base Damage", new List<string>()),
                new Property("Judgement Range", new List<string>()),
                new Property("Punishment Damage", new List<string>()),
                new Property("Punishment Knockback", new List<string>())
            };

            for (int i = 1; i <= level; i++)
            {
                Twilight ability = new Twilight(i);

                ls[0].Values.Add($"{ability.EyesDmg}");
                ls[1].Values.Add($"{ability.EyesBuffDuration / 60:F2}s");
                ls[2].Values.Add($"{ability.EyesCount}");
                ls[3].Values.Add($"{ability.JudgeBaseDmg}");
                ls[4].Values.Add($"{ability.JudgeRange} blocks");
                ls[5].Values.Add($"{ability.PunishDmg}");
                ls[6].Values.Add($"{ability.PunishKB}");

            }

            return ls;
        }

        public void Run(int level)
        {
            Twilight ability = new Twilight(level);

            Console.WriteLine($"Brilliant Eyes Damage: {ability.EyesDmg}");
            Console.WriteLine($"Brilliant Eyes Buffs Duration: {ability.EyesBuffDuration / 60:F2}s");
            Console.WriteLine($"Brilliant Eyes Count: {ability.EyesCount}");
            Console.WriteLine($"Judgement Base Damage: {ability.JudgeBaseDmg}");
            Console.WriteLine($"Judgement Range: {ability.JudgeRange} blocks");
            Console.WriteLine($"Punishment Damage: {ability.PunishDmg}");
            Console.WriteLine($"Punishment Knockback: {ability.PunishKB}");
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