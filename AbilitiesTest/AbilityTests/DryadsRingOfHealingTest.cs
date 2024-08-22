using Abilities;

namespace AbilitiesTest.AbilityTests
{
    public class DryadsRingOfHealingTest : IAbilityTest
    {
        public string Description => "Summons a healing circle around the player. The circle heals everyone inside it and gives them Dryad's Blessing buff.";
        public List<Property> GetStatsAsListTill(int level = 5)
        {
            List<Property> ls = new List<Property>() {
                new Property("Healing Percentage", new List<string>()),
                new Property("Buff Duration", new List<string>()),

            };

            for (int i = 1; i <= level; i++)
            {
                DryadsRingOfHealing ability = new DryadsRingOfHealing(i);

                ls[0].Values.Add($"{ability.HealPercentage * 100:F2}%");
                ls[1].Values.Add($"{ability.BuffDurationInTicks / 60.0:F2}s");

            }

            return ls;
        }

        public void Run(int level)
        {
            DryadsRingOfHealing ab = new DryadsRingOfHealing(level);

            Console.WriteLine($"Buff Duration: {ab.BuffDurationInTicks / 60.0:2}s");
            Console.WriteLine($"Heal Percentage: {ab.HealPercentage}%");
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