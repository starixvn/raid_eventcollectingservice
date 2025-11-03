namespace eventcollectingservice.Models.BWF
{
    /// <summary>
    /// 對應 'bwf_league_mode' table
    /// </summary>
    public class LeagueMode
    {
        public int Id { get; set; }
        public int LeagueId { get; set; }
        public string Name { get; set; } = string.Empty;
        public int LevelId { get; set; }
        public string Mode { get; set; } = string.Empty;
        
        // max_round 允許 Null，所以使用 string?
        public string? MaxRound { get; set; }
        
        public DateTime UpdateTime { get; set; }
        public DateTime CreateTime { get; set; }
    }
}

