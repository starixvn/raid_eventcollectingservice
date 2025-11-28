using System.Text.Json.Serialization;

namespace eventcollectingservice.Models.BetCity
{
    /// <summary>
    /// Base class for BetCity result data containing common fields across all sports
    /// </summary>
    public abstract class BaseBetCityResultData
    {
        // Common identification fields
        [JsonPropertyName("eventId")]
        public int EventId { get; set; }

        [JsonPropertyName("eventDate")]
        public string EventDate { get; set; }

        [JsonPropertyName("eventDateTime")]
        public string EventDateTime { get; set; }

        [JsonPropertyName("leagueGroupId")]
        public int LeagueGroupId { get; set; }

        [JsonPropertyName("leagueId")]
        public int LeagueId { get; set; }

        [JsonPropertyName("league")]
        public string League { get; set; }

        [JsonPropertyName("islive")]
        public int IsLive { get; set; }  // "0" or "1" string, not boolean

        [JsonPropertyName("home")]
        public string Home { get; set; }

        [JsonPropertyName("away")]
        public string Away { get; set; }

        // Common score field for all sports
        [JsonPropertyName("finalscore")]
        public string? FinalScore { get; set; }

        // TODO: Add match status field to enable FT confirmation
        // [JsonPropertyName("match_status")]
        // public string? MatchStatus { get; set; }  // "Live", "Finished", "Ended", "HalfTime", etc.
        //
        // This field is needed to confirm FT (final score) automatically:
        // - When match_status = "Finished" or "Ended", we can safely confirm FT
        // - Currently FT is never auto-confirmed because we lack this information
        //
        // Alternative: Query from betcity_match table's status field instead of adding to DTO
    }
}
