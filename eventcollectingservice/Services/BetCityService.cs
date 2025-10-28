using eventcollectingservice.Models.BetCity;
using eventcollectingservice.Repositories;

namespace eventcollectingservice.Services
{
    public interface IBetCityService
    {
        Task ProcessBetCityEvent(List<BetCityEventData> BetCityEventData);
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
                leagues.Add(new League { Id = eventData.LeagueId, Name = eventData.League, LeagueGroupId = eventData.LeagueGroupId });
                teams.Add(new Team { Id = eventData.HomeId, Name = eventData.Home, LeagueGroupId = eventData.LeagueGroupId, Country = eventData.HomeCountry });
                teams.Add(new Team { Id = eventData.AwayId, Name = eventData.Away, LeagueGroupId = eventData.LeagueGroupId, Country = eventData.AwayCountry });
                matches.Add(new Match { Id = eventData.EventId, Team1Id = eventData.HomeId, Team2Id = eventData.AwayId, KickoffTime = ParseDateTime(eventData.EventDateTime), LeagueId = eventData.LeagueId });
            }

            // Remove duplicates before saving
            leagueGroups = leagueGroups.GroupBy(lg => lg.Id).Select(lg => lg.First()).ToList();
            leagues = leagues.GroupBy(l => l.Id).Select(l => l.First()).ToList();
            teams = teams.GroupBy(t => t.Id).Select(t => t.First()).ToList();
            matches = matches.GroupBy(m => m.Id).Select(m => m.First()).ToList();
            
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
    }
}
