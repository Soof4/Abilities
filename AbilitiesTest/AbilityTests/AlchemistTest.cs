using Abilities;

namespace AbilitiesTest.AbilityTests
{
    public class AlchemistTest : IAbilityTest
    {
        public string Description => "Throws different kinds of potions around the player that lasts for a few seconds in their location:" +
        "\n1: Heal Potion" +
        "\n2: Harm Potion" +
        "\n3: Vortex Potion" +
        "\n4: Curse Potion" +
        "\n5: Shield Potion" +
        "\n6: Power Potion";

        public List<Property> GetStatsAsListTill(int level = 5)
        {
            List<Property> ls = new List<Property>() {
                new Property("Thrown Potion Types", new List<string>()),
                new Property("Potion Size", new List<string>()),
                new Property("Potion Lifetime", new List<string>()),
                new Property("Heal Pot. Heal", new List<string>()),
                new Property("Damage Pot. Damage", new List<string>()),
                new Property("Vortex Pot. Damage", new List<string>()),
                new Property("Curse Pot. Debuff Types", new List<string>()),
                new Property("Curse Pot. Debuffs Duration", new List<string>())
            };

            ls[0].Values.Add("Heal, Damage, Vortex");    // Level 1
            ls[0].Values.Add("Heal, Damage, Vortex, Curse");    // Level 2
            ls[0].Values.Add("Heal, Damage, Vortex, Curse, Shield");    // Level 3
            ls[0].Values.Add("Heal, Damage, Vortex, Curse, Shield, Power");    // Level 4
            ls[0].Values.Add("Heal, Damage, Vortex, Curse, Shield, Power");    // Level 5

            ls[6].Values.Add("Cursed Inferno, Ichor");
            ls[6].Values.Add("Cursed Inferno, Ichor");
            ls[6].Values.Add("Cursed Inferno, Ichor");
            ls[6].Values.Add("Cursed Inferno, Ichor, Oiled, Betsy's Curse");
            ls[6].Values.Add("Cursed Inferno, Ichor, Oiled, Betsy's Curse");


            for (int i = 1; i <= level; i++)
            {
                Alchemist ability = new Alchemist(i);

                ls[1].Values.Add($"{ability.PotionSize}");
                ls[2].Values.Add($"{ability.PotionLifetime}s");
                ls[3].Values.Add($"{ability.Pot1Heal}");
                ls[4].Values.Add($"{ability.Pot2Dmg}");
                ls[5].Values.Add($"{ability.Pot3Dmg}");
                ls[7].Values.Add($"{ability.Pot4Duration / 60.0:F2}s");

                if (i > 5)
                {
                    ls[0].Values.Add(ls[0].Values.Last());
                    ls[6].Values.Add(ls[6].Values.Last());
                }
            }

            return ls;
        }

        public void Run(int level)
        {
            Alchemist ability = new Alchemist(level);

            switch (level)
            {
                case 1:
                    Console.WriteLine("Thrown Potion Types: Heal, Damage, Vortex");    // Level 1
                    break;
                case 2:
                    Console.WriteLine("Thrown Potion Types: Heal, Damage, Vortex, Curse");    // Level 2
                    break;
                case 3:
                    Console.WriteLine("Thrown Potion Types: Heal, Damage, Vortex, Curse, Shield");    // Level 3
                    break;
                default:
                    Console.WriteLine("Thrown Potion Types: Heal, Damage, Vortex, Curse, Shield, Power");    // Level 4 and above
                    break;
            }

            Console.WriteLine($"Potion Size: {ability.PotionSize}");
            Console.WriteLine($"Potion Lifetime: {ability.PotionLifetime}s");
            Console.WriteLine($"Heal Pot. Heal: {ability.Pot1Heal}");
            Console.WriteLine($"Damage Pot. Damage: {ability.Pot2Dmg}");
            Console.WriteLine($"Vortex Pot. Damage: {ability.Pot3Dmg}");

            if (level <= 3)
            {
                Console.WriteLine("Curse Pot. Debuff Types: Cursed Inferno, Ichor");
                Console.WriteLine("Curse Pot. Debuff Types: Cursed Inferno, Ichor");
                Console.WriteLine("Curse Pot. Debuff Types: Cursed Inferno, Ichor");
            }
            else
            {
                Console.WriteLine("Curse Pot. Debuff Types: Cursed Inferno, Ichor, Oiled, Betsy's Curse");
                Console.WriteLine("Curse Pot. Debuff Types: Cursed Inferno, Ichor, Oiled, Betsy's Curse");
            }

            Console.WriteLine($"Curse Pot. Debuffs Duration: {ability.Pot4Duration / 60.0:F2}s");
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