using Abilities;
namespace AbilitiesTest.AbilityTests
{
    public class SetsBlessingTest : IAbilityTest
    {
        public string Description => "This will be written later on.";

        public string GetStatsTableTill(int level = 5)
        {
            List<Property> ls = new List<Property>() {
                new Property("Duration", new List<string>())
            };

            for (int i = 1; i <= level; i++)
            {
                SetsBlessing ability = new SetsBlessing(i);

                ls[0].Values.Add($"{ability.DodgeDurationInSeconds:F2}s");
            }

            return Utils.GetPropertiesAsMarkupTable(ls);
        }

        public void Run(int level)
        {
            SetsBlessing ability = new SetsBlessing(level);

            Console.WriteLine($"Duration: {ability.DodgeDurationInSeconds:F2}s");
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