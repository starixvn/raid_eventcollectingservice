namespace eventcollectingservice.Models.BetCity
{
    public class Team
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int LeagueGroupId { get; set; }
        public string? Country { get; set; }
        public DateTime UpdateTime { get; set; }
        public DateTime CreateTime { get; set; }
    }
}
