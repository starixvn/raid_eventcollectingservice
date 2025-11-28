using System.Text.Json.Serialization;

namespace eventcollectingservice.Models.BetCity
{
    /// <summary>
    /// Soccer-specific result data for BetCity
    /// Contains 35+ soccer-specific fields
    /// </summary>
    public class SoccerResultData : BaseBetCityResultData
    {
        // Basic scores
        [JsonPropertyName("halfscore")]
        public string? HalfScore { get; set; }

        // Penalty and card events
        [JsonPropertyName("penalty")]
        public string? Penalty { get; set; }

        [JsonPropertyName("redCard")]
        public string? RedCard { get; set; }

        [JsonPropertyName("firstPenaltyScorer")]
        public string? FirstPenaltyScorer { get; set; }

        // Goal timing events
        [JsonPropertyName("firstGoalFirstHalf")]
        public string? FirstGoalFirstHalf { get; set; }

        [JsonPropertyName("firstGoalSecondHalf")]
        public string? FirstGoalSecondHalf { get; set; }

        [JsonPropertyName("firstGoal")]
        public string? FirstGoal { get; set; }

        [JsonPropertyName("firstGoalMin")]
        public string? FirstGoalMin { get; set; }

        [JsonPropertyName("secondGoalMin")]
        public string? SecondGoalMin { get; set; }

        // Penalty events by half
        [JsonPropertyName("penaltyFirstHalf")]
        public string? PenaltyFirstHalf { get; set; }

        [JsonPropertyName("penaltySecondHalf")]
        public string? PenaltySecondHalf { get; set; }

        [JsonPropertyName("bothTeamsPenalty")]
        public string? BothTeamsPenalty { get; set; }

        [JsonPropertyName("penaltyMissed")]
        public string? PenaltyMissed { get; set; }

        [JsonPropertyName("penaltyScored")]
        public string? PenaltyScored { get; set; }

        [JsonPropertyName("penaltyVAR")]
        public string? PenaltyVAR { get; set; }

        // Card events by half
        [JsonPropertyName("redCardFirstHalf")]
        public string? RedCardFirstHalf { get; set; }

        [JsonPropertyName("redCardSecondHalf")]
        public string? RedCardSecondHalf { get; set; }

        [JsonPropertyName("redCardBothTeams")]
        public string? RedCardBothTeams { get; set; }

        [JsonPropertyName("goalkeeperRedCard")]
        public string? GoalkeeperRedCard { get; set; }

        // Special events
        [JsonPropertyName("scoreFirst10Min")]
        public string? ScoreFirst10Min { get; set; }

        [JsonPropertyName("arubaFirstPenalty")]
        public string? ArubaFirstPenalty { get; set; }

        [JsonPropertyName("totalPenalties")]
        public string? TotalPenalties { get; set; }

        [JsonPropertyName("goalInTimeRange")]
        public string? GoalInTimeRange { get; set; }

        [JsonPropertyName("twoGoalsInRow")]
        public string? TwoGoalsInRow { get; set; }

        [JsonPropertyName("threeGoalsInRow")]
        public string? ThreeGoalsInRow { get; set; }
    }

    public class SoccerResultRequest
    {
        public List<SoccerResultData>? Results { get; set; }
    }
}
