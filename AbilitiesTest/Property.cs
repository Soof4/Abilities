namespace AbilitiesTest
{
    public class Property
    {
        public string Name { get; set; }
        public List<string> Values { get; set; }

        public Property(string name, List<string> value)
        {
            Name = name;
            Values = value;
        }
    }
}