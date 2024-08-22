using System.Reflection;
using Abilities;

namespace AbilitiesTest.AbilityTests
{
    public class WitchTest : IAbilityTest
    {
        public string Description => "Curse your enemies with debuffs by summoning an ancient magical circle.";

        public List<Property> GetStatsAsListTill(int level = 5)
        {
            List<Property> ls = new List<Property>() {
                new Property("Range", new List<string>()),
                new Property("Debuffs", new List<string>())
            };

            for (int i = 1; i <= level; i++)
            {
                Witch ability = new Witch(i);

                string buffNames = "";

                for (int j = 0; j < ability.BuffTypes.Count; j++)
                {
                    foreach (FieldInfo field in typeof(Terraria.ID.BuffID).GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy))
                    {
                        if (field.IsLiteral && !field.IsInitOnly && field.FieldType == typeof(int))
                        {
                            int fieldValue = (int)field.GetRawConstantValue()!;

                            if (fieldValue == ability.BuffTypes[j])
                            {
                                buffNames += $", {field.Name}";
                            }
                        }
                    }
                }

                buffNames = buffNames.Trim(',').Trim();

                ls[0].Values.Add($"{ability.RangeInBlocks} blocks");
                ls[1].Values.Add($"{buffNames}");
            }

            return ls;
        }

        public void Run(int level)
        {
            Witch ability = new Witch(level);

            string buffNames = "";

            for (int j = 0; j < ability.BuffTypes.Count; j++)
            {
                foreach (FieldInfo field in typeof(Terraria.ID.BuffID).GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy))
                {
                    if (field.IsLiteral && !field.IsInitOnly && field.FieldType == typeof(int))
                    {
                        int fieldValue = (int)field.GetRawConstantValue()!;

                        if (fieldValue == ability.BuffTypes[j])
                        {
                            buffNames += $", {field.Name}";
                        }
                    }
                }
            }

            buffNames = buffNames.Trim(',').Trim();

            Console.WriteLine($"Range: {ability.RangeInBlocks} blocks");
            Console.WriteLine($"Debuffs: {buffNames}");
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