namespace eventcollectingservice.Models.BetCity
{
    /// <summary>
    /// Database entity for storing match results in key-value format
    /// Corresponds to 'betcity_result' table
    /// Each result field is stored as a separate row
    /// Supports Soccer, Basketball, and Volleyball results
    /// </summary>
    public class MatchResult
    {
        public long Id { get; set; }              // Auto-generated primary key
        public int MatchId { get; set; }          // Foreign key to betcity_match
        public string Key { get; set; }           // Result key (e.g., "halfscore", "finalscore", "q1score", etc.)
        public string Value { get; set; }         // Result value
        public bool IsConfirmed { get; set; }     // Whether this result is confirmed
        public DateTime CreateTime { get; set; }  // Creation timestamp
    }
}
