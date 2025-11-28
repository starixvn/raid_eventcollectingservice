using eventcollectingservice.Models.BetCity;
using eventcollectingservice.Repositories;

namespace eventcollectingservice.Services
{
    public interface IBetCityService
    {
        Task ProcessBetCityEvent(List<BetCityEventData> BetCityEventData);
        Task ProcessSoccerResults(List<SoccerResultData> soccerResultData);
        Task ProcessBasketballResults(List<BasketballResultData> basketballResultData);
        Task ProcessVolleyballResults(List<VolleyballResultData> volleyballResultData);
    }
    public class BetCityService : IBetCityService
    {
        private readonly IBetCityRepository _betCityRepository;

        public BetCityService(IBetCityRepository betCityRepository)
        {
            _betCityRepository = betCityRepository;
        }

        public async Task ProcessBetCityEvent(List<BetCityEventData> betCityEvents)
        {
            try
            {
                if (betCityEvents == null || !betCityEvents.Any())
                {
                    throw new ArgumentException("BetCity event data is required and cannot be empty");
                }

                foreach (var eventData in betCityEvents)
                {
                    if (eventData.EventId <= 0)
                    {
                        throw new ArgumentException($"Event ID must be greater than 0. Invalid Event ID: {eventData.EventId}");
                    }
                }

                await ProcessEvents(betCityEvents);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in BetCityService.ProcessBetCityEvent: {ex.Message}");
                throw;
            }
        }

        private async Task ProcessEvents(List<BetCityEventData> events)
        {
            var leagueGroups = new List<LeagueGroup>();
            var leagues = new List<League>();
            var teams = new List<Team>();
            var matches = new List<Match>();

            foreach (var eventData in events)
            {
                if (eventData is { Future: 0, Statistics: 0 })
                {
                    leagueGroups.Add(new LeagueGroup { Id = eventData.LeagueGroupId, Name = eventData.League });
                }
                leagues.Add(new League { Id = eventData.LeagueId, Name = eventData.League, LeagueGroupId = eventData.LeagueGroupId, SportId = eventData.SportId });
                teams.Add(new Team { Id = eventData.HomeId, Name = eventData.Home, LeagueGroupId = eventData.LeagueGroupId, Country = eventData.HomeCountry });
                teams.Add(new Team { Id = eventData.AwayId, Name = eventData.Away, LeagueGroupId = eventData.LeagueGroupId, Country = eventData.AwayCountry });
                matches.Add(new Match { Id = eventData.EventId, Team1Id = eventData.HomeId, Team2Id = eventData.AwayId, KickoffTime = ParseDateTime(eventData.EventDateTime), LeagueId = eventData.LeagueId, IsActive = true });
            }

            // Remove duplicates before saving
            leagueGroups = leagueGroups.GroupBy(lg => lg.Id).Select(lg => lg.First()).ToList();
            leagues = leagues.GroupBy(l => l.Id).Select(l => l.First()).ToList();
            teams = teams.GroupBy(t => t.Id).Select(t => t.First()).ToList();
            matches = matches.GroupBy(m => m.Id).Select(m => m.First()).ToList();

            // Get currently active matches from DB
            var activeMatchIds = await _betCityRepository.GetActiveMatchIds();

            // Find matches that are no longer in the incoming data
            var incomingMatchIds = matches.Select(m => m.Id).ToList();
            var matchesToDeactivate = activeMatchIds.Where(id => !incomingMatchIds.Contains(id)).ToList();

            // Deactivate matches that are no longer present
            if (matchesToDeactivate.Any())
            {
                await _betCityRepository.DeactivateMatches(matchesToDeactivate);
            }

            if (leagueGroups.Any())
            {
                await _betCityRepository.SaveLeagueGroups(leagueGroups);
            }
            await _betCityRepository.SaveLeagues(leagues);
            await _betCityRepository.SaveTeams(teams);
            await _betCityRepository.SaveMatches(matches);
        }

        private DateTime ParseDateTime(string dateTimeString)
        {
            if (DateTime.TryParse(dateTimeString, out DateTime result))
            {
                return result;
            }
            return DateTime.Now;
        }

        public async Task ProcessSoccerResults(List<SoccerResultData> soccerResults)
        {
            try
            {
                if (soccerResults == null || !soccerResults.Any())
                {
                    throw new ArgumentException("Soccer result data is required and cannot be empty");
                }

                // Validate event IDs
                foreach (var resultData in soccerResults)
                {
                    if (resultData.EventId <= 0)
                    {
                        throw new ArgumentException($"Event ID must be greater than 0. Invalid Event ID: {resultData.EventId}");
                    }
                }

                await ProcessSoccerResultsInternal(soccerResults);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in BetCityService.ProcessSoccerResults: {ex.Message}");
                throw;
            }
        }

        public async Task ProcessBasketballResults(List<BasketballResultData> basketballResults)
        {
            try
            {
                if (basketballResults == null || !basketballResults.Any())
                {
                    throw new ArgumentException("Basketball result data is required and cannot be empty");
                }

                // Validate event IDs
                foreach (var resultData in basketballResults)
                {
                    if (resultData.EventId <= 0)
                    {
                        throw new ArgumentException($"Event ID must be greater than 0. Invalid Event ID: {resultData.EventId}");
                    }
                }

                await ProcessBasketballResultsInternal(basketballResults);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in BetCityService.ProcessBasketballResults: {ex.Message}");
                throw;
            }
        }

        public async Task ProcessVolleyballResults(List<VolleyballResultData> volleyballResults)
        {
            try
            {
                if (volleyballResults == null || !volleyballResults.Any())
                {
                    throw new ArgumentException("Volleyball result data is required and cannot be empty");
                }

                // Validate event IDs
                foreach (var resultData in volleyballResults)
                {
                    if (resultData.EventId <= 0)
                    {
                        throw new ArgumentException($"Event ID must be greater than 0. Invalid Event ID: {resultData.EventId}");
                    }
                }

                await ProcessVolleyballResultsInternal(volleyballResults);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in BetCityService.ProcessVolleyballResults: {ex.Message}");
                throw;
            }
        }

        private async Task ProcessSoccerResultsInternal(List<SoccerResultData> resultDataList)
        {
            var matchResults = new List<MatchResult>();

            // Convert each soccer result to key-value pairs
            foreach (var resultData in resultDataList)
            {
                int matchId = resultData.EventId;

                // Common score field
                AddResultIfNotEmpty(matchResults, matchId, "ft", resultData.FinalScore);

                // Soccer-specific fields
                AddResultIfNotEmpty(matchResults, matchId, "ht", resultData.HalfScore);
                AddResultIfNotEmpty(matchResults, matchId, "penalty", resultData.Penalty);
                AddResultIfNotEmpty(matchResults, matchId, "redCard", resultData.RedCard);
                AddResultIfNotEmpty(matchResults, matchId, "firstPenaltyScorer", resultData.FirstPenaltyScorer);
                AddResultIfNotEmpty(matchResults, matchId, "firstGoalFirstHalf", resultData.FirstGoalFirstHalf);
                AddResultIfNotEmpty(matchResults, matchId, "firstGoalSecondHalf", resultData.FirstGoalSecondHalf);
                AddResultIfNotEmpty(matchResults, matchId, "penaltyFirstHalf", resultData.PenaltyFirstHalf);
                AddResultIfNotEmpty(matchResults, matchId, "penaltySecondHalf", resultData.PenaltySecondHalf);
                AddResultIfNotEmpty(matchResults, matchId, "bothTeamsPenalty", resultData.BothTeamsPenalty);
                AddResultIfNotEmpty(matchResults, matchId, "redCardFirstHalf", resultData.RedCardFirstHalf);
                AddResultIfNotEmpty(matchResults, matchId, "redCardSecondHalf", resultData.RedCardSecondHalf);
                AddResultIfNotEmpty(matchResults, matchId, "goalkeeperRedCard", resultData.GoalkeeperRedCard);
                AddResultIfNotEmpty(matchResults, matchId, "penaltyMissed", resultData.PenaltyMissed);
                AddResultIfNotEmpty(matchResults, matchId, "penaltyScored", resultData.PenaltyScored);
                AddResultIfNotEmpty(matchResults, matchId, "penaltyVAR", resultData.PenaltyVAR);
                AddResultIfNotEmpty(matchResults, matchId, "redCardBothTeams", resultData.RedCardBothTeams);
                AddResultIfNotEmpty(matchResults, matchId, "scoreFirst10Min", resultData.ScoreFirst10Min);
                AddResultIfNotEmpty(matchResults, matchId, "arubaFirstPenalty", resultData.ArubaFirstPenalty);
                AddResultIfNotEmpty(matchResults, matchId, "firstGoalMin", resultData.FirstGoalMin);
                AddResultIfNotEmpty(matchResults, matchId, "firstGoal", resultData.FirstGoal);
                AddResultIfNotEmpty(matchResults, matchId, "secondGoalMin", resultData.SecondGoalMin);
                AddResultIfNotEmpty(matchResults, matchId, "totalPenalties", resultData.TotalPenalties);
                AddResultIfNotEmpty(matchResults, matchId, "goalInTimeRange", resultData.GoalInTimeRange);
                AddResultIfNotEmpty(matchResults, matchId, "twoGoalsInRow", resultData.TwoGoalsInRow);
                AddResultIfNotEmpty(matchResults, matchId, "threeGoalsInRow", resultData.ThreeGoalsInRow);
            }

            await SaveProcessedResults(matchResults);
        }

        private async Task ProcessBasketballResultsInternal(List<BasketballResultData> resultDataList)
        {
            var matchResults = new List<MatchResult>();

            // Convert each basketball result to key-value pairs
            foreach (var resultData in resultDataList)
            {
                int matchId = resultData.EventId;

                // Common score field
                AddResultIfNotEmpty(matchResults, matchId, "ft", resultData.FinalScore);

                // Quarter scores
                AddResultIfNotEmpty(matchResults, matchId, "q1score", resultData.Q1Score);
                AddResultIfNotEmpty(matchResults, matchId, "q2score", resultData.Q2Score);
                AddResultIfNotEmpty(matchResults, matchId, "q3score", resultData.Q3Score);
                AddResultIfNotEmpty(matchResults, matchId, "q4score", resultData.Q4Score);

                // Overtime scores
                AddResultIfNotEmpty(matchResults, matchId, "ot1Score", resultData.OT1Score);
                AddResultIfNotEmpty(matchResults, matchId, "ot2Score", resultData.OT2Score);
                AddResultIfNotEmpty(matchResults, matchId, "ot3Score", resultData.OT3Score);

                // Basketball-specific events
                AddResultIfNotEmpty(matchResults, matchId, "firstToScore", resultData.FirstToScore);
                AddResultIfNotEmpty(matchResults, matchId, "race20Q3", resultData.Race20Q3);
                AddResultIfNotEmpty(matchResults, matchId, "race100", resultData.Race100);
                AddResultIfNotEmpty(matchResults, matchId, "race75", resultData.Race75);
                AddResultIfNotEmpty(matchResults, matchId, "race50", resultData.Race50);
            }

            await SaveProcessedResults(matchResults);
        }

        private async Task ProcessVolleyballResultsInternal(List<VolleyballResultData> resultDataList)
        {
            var matchResults = new List<MatchResult>();

            // Convert each volleyball result to key-value pairs
            foreach (var resultData in resultDataList)
            {
                int matchId = resultData.EventId;

                // Common score field
                AddResultIfNotEmpty(matchResults, matchId, "ft", resultData.FinalScore);

                // Set scores
                AddResultIfNotEmpty(matchResults, matchId, "s1score", resultData.Q1Score);
                AddResultIfNotEmpty(matchResults, matchId, "s2score", resultData.Q2Score);
                AddResultIfNotEmpty(matchResults, matchId, "s3score", resultData.Q3Score);
                AddResultIfNotEmpty(matchResults, matchId, "s4score", resultData.Q4Score);
                AddResultIfNotEmpty(matchResults, matchId, "s5score", resultData.Q5Score);
            }

            await SaveProcessedResults(matchResults);
        }

        private async Task SaveProcessedResults(List<MatchResult> matchResults)
        {
            // Remove duplicates by (MatchId, Key) - keep last occurrence
            matchResults = matchResults
                .GroupBy(r => new { r.MatchId, r.Key })
                .Select(g => g.Last())
                .ToList();

            if (!matchResults.Any())
            {
                Console.WriteLine("No valid results to save. All result fields were empty.");
                return;
            }

            // Validate that matches exist before saving results
            var matchIds = matchResults.Select(r => r.MatchId).Distinct().ToList();
            var existingMatchIds = await _betCityRepository.GetExistingMatchIds(matchIds);

            // Filter out results for non-existent matches
            var validResults = matchResults
                .Where(r => existingMatchIds.Contains(r.MatchId))
                .ToList();

            if (validResults.Count < matchResults.Count)
            {
                var invalidMatchIds = matchIds.Except(existingMatchIds).ToList();
                Console.WriteLine($"Warning: {invalidMatchIds.Count} match(es) do not exist in database. IDs: {string.Join(", ", invalidMatchIds)}");
            }

            if (validResults.Any())
            {
                await _betCityRepository.SaveMatchResults(validResults);
            }
            else
            {
                Console.WriteLine("No valid results to save. All match IDs were invalid.");
            }
        }

        private void AddResultIfNotEmpty(List<MatchResult> matchResults, int matchId, string key, string? value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                matchResults.Add(new MatchResult
                {
                    MatchId = matchId,
                    Key = key,
                    Value = value,
                    IsConfirmed = false
                });
            }
        }
    }
}
