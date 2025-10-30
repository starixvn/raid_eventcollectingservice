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


                // 添加聯賽
                var leagueId = await iptvRepository.SaveLeagues(new League { Name = eventData.LeagueName });
                var team1Id = await iptvRepository.SaveTeams(new Team { Name = eventData.HomeTeamName });
                var team2Id = await iptvRepository.SaveTeams(new Team { Name = eventData.AwayTeamName });
                // 添加隊伍


                // 添加比賽
                matches.Add(new GameMatch
                {
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
