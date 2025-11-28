namespace eventcollectingservice.Models.Futbol24
{
    /// <summary>
    /// Request DTO for Futbol24 result data from n8n workflow
    /// </summary>
    public class Futbol24ResultData
    {
        // Match identification
        public int Cid { get; set; }
        public long MId { get; set; }
        public DateTime EventDate { get; set; }
        public DateTime EventTime { get; set; }

        // League info
        public int PLeagueId { get; set; }
        public int LeagueId { get; set; }

        // Team names
        public string HomeName { get; set; }
        public string AwayName { get; set; }

        // Match status
        public int EventStatus { get; set; }

        // Score results (nullable - may not be available yet)
        public string? HT { get; set; }        // Half Time Score (e.g., "1-0")
        public string? FT { get; set; }        // Full Time Score (e.g., "2-1")
        public string? AET { get; set; }       // After Extra Time Score (e.g., "2-2")
        public string? AP { get; set; }        // After Penalties Score (e.g., "5-4")

        // Cards (nullable)
        public int? HRedCard { get; set; }     // Home Red Card count
        public int? ARedCard { get; set; }     // Away Red Card count

        // Match info (P-P: Postponed, CANC: Cancelled, w.o.: Walkover)
        public string? Info { get; set; }
    }

    public class Futbol24ResultRequest
    {
        public List<Futbol24ResultData>? Results { get; set; }
    }
}
