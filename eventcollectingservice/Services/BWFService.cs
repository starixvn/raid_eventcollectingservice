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

                // 添加 LeagueMode
                var leagueModeId = await _bwfRepository.SaveLeagueMode(new LeagueMode
                {
                    LeagueId = matchData.LeagueId,
                    Name = matchData.LeagueName,
                    LevelId = levelId,
                    Mode = matchData.Mode,
                    AgeCategory = matchData.AgeCategory,
                    MaxRound = matchData.MaxRound
                });

                matchesToSave.Add(new Match
                {
                    Id = matchData.MatchId,
                    KickoffTime = matchData.KickoffTime,
                    PlayerHome1Id = matchData.PlayerHome1Id,
                    PlayerHome2Id = matchData.PlayerHome2Id,
                    PlayerAway1Id = matchData.PlayerAway1Id,
                    PlayerAway2Id = matchData.PlayerAway2Id,
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

