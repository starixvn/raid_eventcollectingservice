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
            var categories = new List<Category>();
            var leagues = new List<League>();
            var teams = new List<Team>();
            var matches = new List<GameMatch>();

            foreach (var eventData in events)
            {
                categories.Add(new Category { Id = eventData.CId, Name = eventData.CName });

                leagues.Add(new League { Id = eventData.PLeagueId, Name = eventData.PLeagueName, CategoryId = eventData.CId });
                if (eventData.LeagueId != eventData.PLeagueId && !string.IsNullOrEmpty(eventData.LeagueName))
                {
                    leagues.Add(new League { Id = eventData.LeagueId, Name = eventData.LeagueName, CategoryId = eventData.CId, ParentLeagueId = eventData.PLeagueId });
                }

                teams.Add(new Team { Id = eventData.HomeId, Name = eventData.HomeName, LeagueId = eventData.PLeagueId });
                teams.Add(new Team { Id = eventData.AwayId, Name = eventData.AwayName, LeagueId = eventData.PLeagueId });

                matches.Add(new GameMatch { Id = (int)eventData.Mid, Team1Id = eventData.HomeId, Team2Id = eventData.AwayId, KickoffTime = eventData.EventTime, LeagueId = eventData.LeagueId, IsActive = true });
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
                await futbol24Repository.SaveCategories(categories);
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
    }
}
