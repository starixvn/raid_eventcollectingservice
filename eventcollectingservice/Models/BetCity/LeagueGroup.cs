namespace eventcollectingservice.Models.BetCity
{
    public class LeagueGroup
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public DateTime UpdateTime { get; set; }
        public DateTime CreateTime { get; set; }
    }
}
