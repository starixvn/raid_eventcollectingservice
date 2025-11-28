using eventcollectingservice.Models.Futbol24;
using eventcollectingservice.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eventcollectingservice.Services
{
    public interface IFutbol24Service
    {
        Task ProcessFutbol24Event(List<Futbol24EventData> futbol24EventData);
        Task ProcessFutbol24Results(List<Futbol24ResultData> futbol24ResultData);
    }
    public class Futbol24Service(IFutbol24Repository futbol24Repository) : IFutbol24Service
    {
        public async Task ProcessFutbol24Event(List<Futbol24EventData> futbol24Events)
        {
            try
            {
                if (futbol24Events == null || !futbol24Events.Any())
                {
                    throw new ArgumentException("Futbol24 event data is required and cannot be empty");
                }

                foreach (var eventData in futbol24Events)
                {
                    if (eventData.Mid <= 0)
                    {
                        throw new ArgumentException($"Event ID must be greater than 0. Invalid Event ID: {eventData.Mid}");
                    }
                }

                await ProcessEvents(futbol24Events);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in Futbol24Service.ProcessFutbol24Event: {ex.Message}");
                throw;
            }
        }

        private async Task ProcessEvents(List<Futbol24EventData> events)
        {
            var categories = new List<Country>();
            var leagues = new List<League>();
            var teams = new List<Team>();
            var matches = new List<GameMatch>();

            foreach (var eventData in events)
            {
                categories.Add(new Country { Id = eventData.CId, Name = eventData.CName });

                leagues.Add(new League { Id = eventData.PLeagueId, Name = eventData.PLeagueName, CountryId = eventData.CId });
                if (eventData.LeagueId != eventData.PLeagueId && !string.IsNullOrEmpty(eventData.LeagueName))
                {
                    leagues.Add(new League { Id = eventData.LeagueId, Name = eventData.LeagueName, CountryId = eventData.CId, ParentLeagueId = eventData.PLeagueId });
                }

                teams.Add(new Team { Id = eventData.HomeId, Name = eventData.HomeName, LeagueId = eventData.PLeagueId });
                teams.Add(new Team { Id = eventData.AwayId, Name = eventData.AwayName, LeagueId = eventData.PLeagueId });

                matches.Add(new GameMatch { Id = (int)eventData.Mid, Team1Id = eventData.HomeId, Team2Id = eventData.AwayId, KickoffTime = eventData.EventTime, LeagueId = eventData.LeagueId, IsActive = true, Status = eventData.EventStatus });
            }

            // Remove duplicates before saving
            categories = categories.GroupBy(c => c.Id).Select(c => c.First()).ToList();
            leagues = leagues.GroupBy(l => l.Id).Select(l => l.First()).ToList();
            teams = teams.GroupBy(t => t.Id).Select(t => t.First()).ToList();
            matches = matches.GroupBy(m => m.Id).Select(m => m.First()).ToList();

            // Get currently active matches from DB
            var activeMatchIds = await futbol24Repository.GetActiveMatchIds();

            // Find matches that are no longer in the incoming data
            var incomingMatchIds = matches.Select(m => m.Id).ToList();
            var matchesToDeactivate = activeMatchIds.Where(id => !incomingMatchIds.Contains(id)).ToList();

            // Deactivate matches that are no longer present
            if (matchesToDeactivate.Any())
            {
                await futbol24Repository.DeactivateMatches(matchesToDeactivate);
            }

            try
            {
                await futbol24Repository.SaveCountries(categories);
                await futbol24Repository.SaveLeagues(leagues);
                await futbol24Repository.SaveTeams(teams);
                await futbol24Repository.SaveMatches(matches);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task ProcessFutbol24Results(List<Futbol24ResultData> futbol24Results)
        {
            try
            {
                if (futbol24Results == null || !futbol24Results.Any())
                {
                    throw new ArgumentException("Futbol24 result data is required and cannot be empty");
                }

                // Validate match IDs
                foreach (var resultData in futbol24Results)
                {
                    if (resultData.MId <= 0)
                    {
                        throw new ArgumentException($"Match ID must be greater than 0. Invalid Match ID: {resultData.MId}");
                    }
                }

                await ProcessResults(futbol24Results);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in Futbol24Service.ProcessFutbol24Results: {ex.Message}");
                throw;
            }
        }

        private async Task ProcessResults(List<Futbol24ResultData> resultDataList)
        {
            var matchResults = new List<MatchResult>();

            // Convert each result data to multiple key-value pairs
            foreach (var resultData in resultDataList)
            {
                int matchId = (int)resultData.MId;

                // Add HT (Half Time) result if available
                if (!string.IsNullOrEmpty(resultData.HT))
                {
                    matchResults.Add(new MatchResult
                    {
                        MatchId = matchId,
                        Key = "ht",
                        Value = resultData.HT
                    });
                }

                // Add FT (Full Time) result if available
                if (!string.IsNullOrEmpty(resultData.FT))
                {
                    matchResults.Add(new MatchResult
                    {
                        MatchId = matchId,
                        Key = "ft",
                        Value = resultData.FT
                    });
                }

                // Add AET (After Extra Time) result if available
                if (!string.IsNullOrEmpty(resultData.AET))
                {
                    matchResults.Add(new MatchResult
                    {
                        MatchId = matchId,
                        Key = "et",
                        Value = resultData.AET
                    });
                }

                // Add AP (After Penalties) result if available
                if (!string.IsNullOrEmpty(resultData.AP))
                {
                    matchResults.Add(new MatchResult
                    {
                        MatchId = matchId,
                        Key = "ps",
                        Value = resultData.AP
                    });
                }

                // Add HRedCard (Home Red Card) if available
                if (resultData.HRedCard.HasValue)
                {
                    matchResults.Add(new MatchResult
                    {
                        MatchId = matchId,
                        Key = "HRedCard",
                        Value = resultData.HRedCard.Value.ToString()
                    });
                }

                // Add ARedCard (Away Red Card) if available
                if (resultData.ARedCard.HasValue)
                {
                    matchResults.Add(new MatchResult
                    {
                        MatchId = matchId,
                        Key = "ARedCard",
                        Value = resultData.ARedCard.Value.ToString()
                    });
                }

                // Add Info (Match status info) if available
                if (!string.IsNullOrEmpty(resultData.Info))
                {
                    matchResults.Add(new MatchResult
                    {
                        MatchId = matchId,
                        Key = "Info",
                        Value = resultData.Info
                    });
                }
            }

            // Remove duplicates by (MatchId, Key) - keep last occurrence
            matchResults = matchResults
                .GroupBy(r => new { r.MatchId, r.Key })
                .Select(g => g.Last())
                .ToList();

            // Set IsConfirmed status BEFORE saving
            SetFutbol24ConfirmationStatus(matchResults);

            if (!matchResults.Any())
            {
                Console.WriteLine("No valid results to save. All result fields were empty.");
                return;
            }

            // Validate that matches exist before saving results
            var matchIds = matchResults.Select(r => r.MatchId).Distinct().ToList();
            var existingMatchIds = await futbol24Repository.GetExistingMatchIds(matchIds);

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
                await futbol24Repository.SaveMatchResults(validResults);
            }
            else
            {
                Console.WriteLine("No valid results to save. All match IDs were invalid.");
            }
        }

        // ========== Confirmation Status Setting (Before Save) ==========

        private void SetFutbol24ConfirmationStatus(List<MatchResult> matchResults)
        {
            // Group by match_id
            var matchIds = matchResults.Select(r => r.MatchId).Distinct().ToList();

            foreach (var matchId in matchIds)
            {
                // Get keys from current batch only
                var currentKeys = matchResults.Where(r => r.MatchId == matchId).Select(r => r.Key).ToHashSet();

                // Get values for Info key to check match status
                var infoResult = matchResults.FirstOrDefault(r => r.MatchId == matchId && r.Key == "Info");
                string? infoValue = infoResult?.Value;

                // Determine which keys should be confirmed based on current batch
                foreach (var result in matchResults.Where(r => r.MatchId == matchId))
                {
                    result.IsConfirmed = ShouldFutbol24ResultBeConfirmed(result.Key, currentKeys, infoValue);
                }
            }
        }

        private bool ShouldFutbol24ResultBeConfirmed(string key, HashSet<string> currentKeys, string? infoValue)
        {
            // Rule 1: If match is cancelled or postponed, confirm all results
            if (!string.IsNullOrEmpty(infoValue) && (infoValue == "CANC" || infoValue == "P-P"))
            {
                return true;
            }

            // Rule 2: If PS (penalties) exists, confirm FT, ET, and HT
            if (currentKeys.Contains("ps"))
            {
                var psConfirmedKeys = new HashSet<string> { "ft", "et", "ht" };
                if (psConfirmedKeys.Contains(key))
                {
                    return true;
                }
            }

            // Rule 3: If FT exists, confirm HT and ET
            if (currentKeys.Contains("ft"))
            {
                var ftConfirmedKeys = new HashSet<string> { "ht", "et" };
                if (ftConfirmedKeys.Contains(key))
                {
                    return true;
                }
            }

            // Default: not confirmed
            return false;
        }
    }
}
