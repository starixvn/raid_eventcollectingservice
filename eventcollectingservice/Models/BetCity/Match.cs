namespace eventcollectingservice.Models.BetCity
{
    public class Match
    {
        public int Id { get; set; }
        public int Team1Id { get; set; }
        public int Team2Id { get; set; }
        public bool IsActive { get; set; }
        public int Status { get; set; }
        public DateTime KickoffTime { get; set; }
        public int LeagueId { get; set; }
        public DateTime UpdateTime { get; set; }
        public DateTime CreateTime { get; set; }
    }
}
