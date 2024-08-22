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

        public static string GetPropertiesAsHTMLTable(List<Property> ls)
        {
            string table = "<table class=\"statTable\">";

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
            table += "\n  <thead class=\"statsHead\">";
            table += "\n    <th class=\"statsHeadCell\">Propeties</th>";
            for (int i = 1; i <= maxLevelNum; i++)
            {
                table += $"\n    <th class=\"statsHeadCell\">Level {i}</th>";
            }
            table += "\n  </thead>";

            // Concat the property rows
            table += "\n  <tbody class=\"statsBody\">";
            foreach (Property p in ls)
            {
                table += "\n    <tr class=\"statsRow\">";
                table += $"\n    <td class=\"statsCell\">{p.Name}</td>";

                int i = 1;

                foreach (string v in p.Values)
                {
                    table += $"\n      <td class=\"statsCell\">{v}</td>";
                    i++;
                }

                for (; i <= maxLevelNum; i++)
                {
                    table += $"\n      <td class=\"statsCell\">N/A</td>";
                }

                table += "\n    </tr>";
            }

            table += "\n  </tbody>";
            table += "\n</table>";

            return table;
        }
    }
}
