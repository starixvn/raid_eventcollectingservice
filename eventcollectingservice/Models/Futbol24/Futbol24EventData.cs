namespace eventcollectingservice.Models.Futbol24
{
    public class Futbol24EventData
    {
        // 欄位: CId (e.g., 61)
        public int CId { get; set; }

        // 欄位: CName (e.g., "Georgia")
        public string CName { get; set; }

        // 欄位: Mid (e.g., 3086802) - 使用 long 以確保 ID 範圍
        public long Mid { get; set; }
        
        // 欄位: EventTime (e.g., "2025/10-28 0:00")
        public DateTime EventTime { get; set; }

        // 欄位: PLeagueId (e.g., 80797)
        public int PLeagueId { get; set; }

        // 欄位: LeagueId (e.g., 80797)
        public int LeagueId { get; set; }

        // 欄位: PLeagueName (e.g., "Erovnuli Liga")
        public string PLeagueName { get; set; }

        // 欄位: LeagueName (e.g., "Group B", "League Phase", 或空白)
        public string LeagueName { get; set; }

        // 欄位: HomeId (e.g., 2849)
        public int HomeId { get; set; }

        // 欄位: HomeName (e.g., "Dinamo Tbilisi")
        public string HomeName { get; set; }

        // 欄位: AwayId (e.g., 2856)
        public int AwayId { get; set; }

        // 欄位: AwayName (e.g., "Kolkheti Poti")
        public string AwayName { get; set; }
    }

    public class Futbol24EventRequest
    {
        public List<Futbol24EventData>? Events { get; set; }
    }
}
