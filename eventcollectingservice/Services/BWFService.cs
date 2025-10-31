using eventcollectingservice.Models.BWF;
using eventcollectingservice.Repositories;

namespace eventcollectingservice.Services
{
    public interface IBWFService
    {
        Task ProcessBWFMatch(List<BWFMatchData> bwfMatches);
    }

    public class BWFService : IBWFService
    {
        private readonly IBWFRepository _bwfRepository;

        public BWFService(IBWFRepository bwfRepository)
        {
            _bwfRepository = bwfRepository;
        }

        public async Task ProcessBWFMatch(List<BWFMatchData> bwfMatches)
        {
            try
            {
                if (bwfMatches == null || !bwfMatches.Any())
                {
                    throw new ArgumentException("BWF match data is required and cannot be empty");
                }

                // 驗證必要欄位
                foreach (var matchData in bwfMatches)
                {
                    if (matchData.LeagueId <= 0)
                    {
                        throw new ArgumentException($"League ID must be greater than 0. Invalid League ID: {matchData.LeagueId}");
                    }
                }

                await ProcessMatches(bwfMatches);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in BWFService.ProcessBWFMatch: {ex.Message}");
                throw;
            }
        }

        private async Task ProcessMatches(List<BWFMatchData> matchDataList)
        {
            var matchesToSave = new List<Match>();

            // 從事件資料中提取各個實體
            foreach (var matchData in matchDataList)
            {
                // 添加 Level
                var levelId = await _bwfRepository.SaveLevel(new Level { Name = matchData.LevelName });

                // 添加 Player
                var playerHome1Id = await _bwfRepository.SavePlayer(new Player 
                { 
                    Name = matchData.PlayerHome1Name, 
                    Country = matchData.PlayerHome1Country 
                });
                
                int? playerHome2Id = null;
                if (!string.IsNullOrEmpty(matchData.PlayerHome2Name))
                {
                    playerHome2Id = await _bwfRepository.SavePlayer(new Player 
                    { 
                        Name = matchData.PlayerHome2Name, 
                        Country = matchData.PlayerHome2Country ?? "" 
                    });
                }

                var playerAway1Id = await _bwfRepository.SavePlayer(new Player 
                { 
                    Name = matchData.PlayerAway1Name, 
                    Country = matchData.PlayerAway1Country 
                });
                
                int? playerAway2Id = null;
                if (!string.IsNullOrEmpty(matchData.PlayerAway2Name))
                {
                    playerAway2Id = await _bwfRepository.SavePlayer(new Player 
                    { 
                        Name = matchData.PlayerAway2Name, 
                        Country = matchData.PlayerAway2Country ?? "" 
                    });
                }

                // 添加 LeagueMode
                var leagueModeId = await _bwfRepository.SaveLeagueMode(new LeagueMode
                {
                    LeagueId = matchData.LeagueId,
                    Name = matchData.LeagueName,
                    LevelId = levelId,
                    Mode = matchData.Mode,
                    MaxRound = matchData.MaxRound
                });
                
                matchesToSave.Add(new Match 
                { 
                    KickoffTime = matchData.KickoffTime, 
                    PlayerHome1Id = playerHome1Id, 
                    PlayerHome2Id = playerHome2Id, 
                    PlayerAway1Id = playerAway1Id, 
                    PlayerAway2Id = playerAway2Id, 
                    LeagueModeId = leagueModeId, 
                    Court = matchData.Court, 
                    Round = matchData.Round, 
                    CurrentRound = matchData.CurrentRound, 
                    IsActive = matchData.IsActive, 
                    Status = matchData.Status 
                });
            }

            try
            {
                if (matchesToSave.Any())
                {
                    await _bwfRepository.SaveMatches(matchesToSave);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error in BWFService.ProcessMatches: {e.Message}");
                throw;
            }
        }
    }
}

