namespace AbilitiesTest.AbilityTests
{
    public interface IAbilityTest
    {
        public string Description { get; }
        public void Run(int level);
        public void RunTill(int level = 5);
        public List<Property> GetStatsAsListTill(int level = 5);
    }
}