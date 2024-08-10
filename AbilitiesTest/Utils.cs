namespace AbilitiesTest
{
    public static class Utils
    {
        public static string GetPropertiesAsMarkupTable(List<Property> ls)
        {
            string table = "| Properties |";

            // Find the property with highest number of levels
            int maxLevelNum = 0;
            foreach (Property p in ls)
            {
                if (p.Values.Count > maxLevelNum)
                {
                    maxLevelNum = p.Values.Count;
                }
            }

            // Concat the levels row
            for (int i = 1; i <= maxLevelNum; i++)
            {
                table += $" Level {i} |";
            }
            table += "\n";

            // Concat the title and cell diff row
            table += "|-|";
            for (int i = 0; i < maxLevelNum; i++)
            {
                table += "-|";
            }
            table += "\n";

            // Concat the property rows
            foreach (Property p in ls)
            {
                table += $"| {p.Name} |";

                int i = 1;

                foreach (string v in p.Values)
                {
                    table += $" {v} |";
                    i++;
                }

                for (; i <= maxLevelNum; i++)
                {
                    table += $" N/A |";
                }

                table += "\n";
            }

            return table;
        }
    }
}


/*
                  lvl 1 : lvl 2 : lvl 3 : lvl 4 : lvl 5 : ...
Heal Percentage : 0.05% : 0.06% : 0.07% : 0.08% : 0.09% : ...
Buff duration   : 2s    : 3s    : 4s    : 5s    : 6s    : ...
*/