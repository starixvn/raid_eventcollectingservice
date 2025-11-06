using eventcollectingservice.Models.IPTV;
using eventcollectingservice.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eventcollectingservice.Services
{
    public interface IIPTVService
    {
        Task ProcessIPTVEvent(List<IPTVEventData> iptvEventData);
    }

    public class IPTVService(IIPTVRepository iptvRepository) : IIPTVService
    {
        public async Task ProcessIPTVEvent(List<IPTVEventData> iptvEvents)
        {
            try
            {
                if (iptvEvents == null || !iptvEvents.Any())
                {
                    throw new ArgumentException("IPTV event data is required and cannot be empty");
                }

                await ProcessEvents(iptvEvents);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in IPTVService.ProcessIPTVEvent: {ex.Message}");
                throw;
            }
        }

        private async Task ProcessEvents(List<IPTVEventData> events)
        {
            var matches = new List<GameMatch>();

            foreach (var eventData in events)
            {
                // 添加分類
                var eventInfo = await iptvRepository.SearchLeague(eventData);
                int leagueId = 0;
                int team1Id = 0;
                int team2Id = 0;
                if (eventInfo == null)
                {
                    // 添加聯賽
                    leagueId = await iptvRepository.SaveLeagues(new League { Name = eventData.LeagueName });
                    team1Id = await iptvRepository.SaveTeams(new Team { Name = eventData.HomeTeamName });
                    team2Id = await iptvRepository.SaveTeams(new Team { Name = eventData.AwayTeamName });
                }else{
                    leagueId = eventInfo.League_Id;
                    team1Id = eventInfo.Team1_Id;
                    team2Id = eventInfo.Team2_Id;
                }

                // 添加隊伍


                // 添加比賽
                matches.Add(new GameMatch
                {
                    EventId = eventData.EventId,
                    Team1Id = team1Id,
                    Team2Id = team2Id,
                    KickoffTime = eventData.KickoffTime,
                    LeagueId = leagueId,
                    Channel = eventData.Channel,
                    Status = eventData.Status,
                    Alive = eventData.Alive,

                });
            }

            try
            {
                await iptvRepository.SaveMatches(matches);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

    }
}
