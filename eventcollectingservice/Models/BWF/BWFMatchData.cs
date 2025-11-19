namespace eventcollectingservice.Models.BWF
{
    /// <summary>
    /// BWF 賽事資料請求模型
    /// </summary>
    public class BWFMatchData
    {
        // Match 基本資訊
        public ulong MatchId { get; set; }
        public DateTime KickoffTime { get; set; }
        public string? Court { get; set; }
        public string Round { get; set; } = string.Empty;
        public string CurrentRound { get; set; } = string.Empty;
        public bool IsActive { get; set; } = true;
        public bool Status { get; set; } = false;

        // League 資訊
        public int LeagueId { get; set; }
        public string LeagueName { get; set; } = string.Empty;
        public string Mode { get; set; } = string.Empty;  // MS, WS, MD, WD, XD
        public string AgeCategory { get; set; } = string.Empty;
        public string? MaxRound { get; set; }

        // Level 資訊
        public string? LevelName { get; set; }

        // 主場球員資訊
        public int? PlayerHome1Id { get; set; }
        public string PlayerHome1Name { get; set; } = string.Empty;
        public string PlayerHome1Country { get; set; } = string.Empty;
        public int? PlayerHome2Id { get; set; }
        public string? PlayerHome2Name { get; set; }
        public string? PlayerHome2Country { get; set; }

        // 客場球員資訊
        public int? PlayerAway1Id { get; set; }
        public string PlayerAway1Name { get; set; } = string.Empty;
        public string PlayerAway1Country { get; set; } = string.Empty;
        public int? PlayerAway2Id { get; set; }
        public string? PlayerAway2Name { get; set; }
        public string? PlayerAway2Country { get; set; }
    }

    /// <summary>
    /// BWF 賽事請求包裝類
    /// </summary>
    public class BWFMatchesRequest
    {
        public List<BWFMatchData>? Events { get; set; }
    }
}

