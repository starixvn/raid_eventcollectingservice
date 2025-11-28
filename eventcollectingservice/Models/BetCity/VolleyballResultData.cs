using System.Text.Json.Serialization;

namespace eventcollectingservice.Models.BetCity
{
    /// <summary>
    /// Volleyball-specific result data for BetCity
    /// Contains set scores (up to 5 sets)
    /// </summary>
    public class VolleyballResultData : BaseBetCityResultData
    {
        // Set scores (Volleyball can have up to 5 sets)
        [JsonPropertyName("q1score")]
        public string? Q1Score { get; set; }  // Set 1

        [JsonPropertyName("q2score")]
        public string? Q2Score { get; set; }  // Set 2

        [JsonPropertyName("q3score")]
        public string? Q3Score { get; set; }  // Set 3

        [JsonPropertyName("q4score")]
        public string? Q4Score { get; set; }  // Set 4

        [JsonPropertyName("q5score")]
        public string? Q5Score { get; set; }  // Set 5
    }

    public class VolleyballResultRequest
    {
        public List<VolleyballResultData>? Results { get; set; }
    }
}
