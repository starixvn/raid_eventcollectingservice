namespace eventcollectingservice.Models.BWF
{
    /// <summary>
    /// 對應 'bwf_match' table
    /// </summary>
    public class Match
    {
        public ulong Id { get; set; }
        public int PlayerHome1Id { get; set; }

        // player_home_2_id 允許 Null (雙打比賽才有第二位球員)
        public int? PlayerHome2Id { get; set; }

        public int PlayerAway1Id { get; set; }

        // player_away_2_id 允許 Null (雙打比賽才有第二位球員)
        public int? PlayerAway2Id { get; set; }

        public DateTime KickoffTime { get; set; }
        public int LeagueModeId { get; set; }

        // court 允許 Null
        public string? Court { get; set; }

        public string Round { get; set; } = string.Empty;
        public string CurrentRound { get; set; } = string.Empty;

        // is_active: 比賽是否被刪除
        public bool IsActive { get; set; }

        // status: 比賽是否結束
        public bool Status { get; set; }

        public DateTime UpdateTime { get; set; }
        public DateTime CreateTime { get; set; }
    }
}

