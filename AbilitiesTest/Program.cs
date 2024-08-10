using System.Reflection;
using Abilities;
using AbilitiesTest.AbilityTests;

namespace AbilitiesTest
{
    public class Program
    {
        public static void Main(string[] args)
        {
            if (args.Length > 0)
            {
                switch (args.First().ToLower().Trim())
                {
                    case "-md":
                    case "-markdown":
                        ToMarkdown();
                        break;
                }
            }
        }

        public static void ToConsole()
        {
            var assembly = Assembly.GetExecutingAssembly();
            var types = assembly.GetTypes();
            var testTypes = types.Where(t =>
                t.Namespace == "AbilitiesTest.AbilityTests" &&
                !t.IsAbstract && !t.IsInterface
            ).ToList();

            foreach (var test in testTypes)
            {
                IAbilityTest instance = (IAbilityTest)Activator.CreateInstance(test)!;
                string table = $"{test.Name.Remove(test.Name.Length - 4)}\n";
                table += instance.GetStatsTableTill(5);
                Console.WriteLine(table);
            }
        }

        public static void ToMarkdown()
        {
            FileStream fs = File.Create("Stats.md");
            StreamWriter sw = new StreamWriter(fs);

            var assembly = Assembly.GetExecutingAssembly();
            var types = assembly.GetTypes();
            var testTypes = types.Where(t =>
                t.Namespace == "AbilitiesTest.AbilityTests" &&
                !t.IsAbstract && !t.IsInterface
            ).ToList();

            foreach (var test in testTypes)
            {
                IAbilityTest instance = (IAbilityTest)Activator.CreateInstance(test)!;
                string table = $"### {test.Name.Remove(test.Name.Length - 4)}\n";
                table += $"{instance.Description}\n";
                table += instance.GetStatsTableTill(5);
                table += "<br>";
                sw.WriteLine(table);
            }

            sw.Close();
            fs.Close();
        }
    }
}