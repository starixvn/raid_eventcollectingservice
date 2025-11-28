using System.Text.Json.Serialization;

namespace eventcollectingservice.Models.BetCity
{
    /// <summary>
    /// Basketball-specific result data for BetCity
    /// Contains quarter scores, overtime scores, and basketball-specific events
    /// </summary>
    public class BasketballResultData : BaseBetCityResultData
    {
        // Quarter scores
        [JsonPropertyName("q1score")]
        public string? Q1Score { get; set; }

        [JsonPropertyName("q2score")]
        public string? Q2Score { get; set; }

        [JsonPropertyName("q3score")]
        public string? Q3Score { get; set; }

        [JsonPropertyName("q4score")]
        public string? Q4Score { get; set; }

        // Overtime scores
        [JsonPropertyName("ot1Score")]
        public string? OT1Score { get; set; }

        [JsonPropertyName("ot2Score")]
        public string? OT2Score { get; set; }

        [JsonPropertyName("ot3Score")]
        public string? OT3Score { get; set; }

        // Basketball-specific events
        [JsonPropertyName("firstToScore")]
        public string? FirstToScore { get; set; }

        [JsonPropertyName("race20Q3")]
        public string? Race20Q3 { get; set; }      // 20 points race in 3rd quarter

        [JsonPropertyName("race100")]
        public string? Race100 { get; set; }       // 100 points race

        [JsonPropertyName("race75")]
        public string? Race75 { get; set; }        // 75 points race

        [JsonPropertyName("race50")]
        public string? Race50 { get; set; }        // 50 points race
    }

    public class BasketballResultRequest
    {
        public List<BasketballResultData>? Results { get; set; }
    }
}
