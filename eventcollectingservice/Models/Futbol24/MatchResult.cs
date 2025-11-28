namespace eventcollectingservice.Models.Futbol24
{
    /// <summary>
    /// Database entity for storing match results in key-value format
    /// Corresponds to 'futbol24_result' table
    /// Each result field (HT, FT, etc.) is stored as a separate row
    /// </summary>
    public class MatchResult
    {
        public long Id { get; set; }              // Auto-generated primary key
        public int MatchId { get; set; }          // Foreign key to futbol24_match
        public string Key { get; set; }           // Result key: "HT", "FT", "AET", "AP", "HRedCard", "ARedCard", "Info"
        public string Value { get; set; }         // Result value: score string or count
        public bool IsConfirmed { get; set; }     // Whether this result is confirmed
        public DateTime CreateTime { get; set; }  // Creation timestamp
    }
}
