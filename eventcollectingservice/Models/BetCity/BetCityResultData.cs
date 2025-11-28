namespace eventcollectingservice.Models.BetCity
{
    /// <summary>
    /// Request DTO for BetCity result data from n8n workflow
    /// Supports multiple sports: Soccer, Basketball, Volleyball
    /// </summary>
    public class BetCityResultData
    {
        // Common fields for all sports
        public int EventId { get; set; }
        public DateTime EventDate { get; set; }
        public DateTime EventDateTime { get; set; }
        public int LeagueGroupId { get; set; }
        public int LeagueId { get; set; }
        public string League { get; set; }
        public bool IsLive { get; set; }
        public string Home { get; set; }
        public string Away { get; set; }

        // Common score fields (Soccer, Basketball, Volleyball)
        public string? FinalScore { get; set; }

        // Soccer-specific fields
        public string? HalfScore { get; set; }
        public string? Penalty { get; set; }
        public string? RedCard { get; set; }
        public string? FirstPenaltyScorer { get; set; }
        public string? FirstGoalFirstHalf { get; set; }
        public string? FirstGoalSecondHalf { get; set; }
        public string? PenaltyFirstHalf { get; set; }
        public string? PenaltySecondHalf { get; set; }
        public string? BothTeamsPenalty { get; set; }
        public string? RedCardFirstHalf { get; set; }
        public string? RedCardSecondHalf { get; set; }
        public string? GoalkeeperRedCard { get; set; }
        public string? PenaltyMissed { get; set; }
        public string? PenaltyScored { get; set; }
        public string? PenaltyVAR { get; set; }
        public string? RedCardBothTeams { get; set; }
        public string? ScoreFirst10Min { get; set; }
        public string? ArubaFirstPenalty { get; set; }
        public string? FirstGoalMin { get; set; }
        public string? FirstGoal { get; set; }
        public string? SecondGoalMin { get; set; }
        public string? TotalPenalties { get; set; }
        public string? GoalInTimeRange { get; set; }
        public string? TwoGoalsInRow { get; set; }
        public string? ThreeGoalsInRow { get; set; }

        // Basketball & Volleyball quarter/set scores
        public string? Q1Score { get; set; }
        public string? Q2Score { get; set; }
        public string? Q3Score { get; set; }
        public string? Q4Score { get; set; }
        public string? Q5Score { get; set; }  // Volleyball only

        // Basketball overtime scores
        public string? OT1Score { get; set; }
        public string? OT2Score { get; set; }
        public string? OT3Score { get; set; }

        // Basketball-specific events
        public string? FirstToScore { get; set; }
        public string? Race20Q3 { get; set; }
        public string? Race100 { get; set; }
        public string? Race75 { get; set; }
        public string? Race50 { get; set; }
    }

    public class BetCityResultRequest
    {
        public List<BetCityResultData>? Results { get; set; }
    }
}
