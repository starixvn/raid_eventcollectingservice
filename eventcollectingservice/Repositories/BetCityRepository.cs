using System.Data;
using Dapper;
using eventcollectingservice.Models.BetCity;
using MySql.Data.MySqlClient;

namespace eventcollectingservice.Repositories
{
    public interface IBetCityRepository
    {
        Task SaveLeagueGroups(List<LeagueGroup> leagueGroups);
        Task SaveLeagues(List<League> leagues);
        Task SaveTeams(List<Team> teams);
        Task SaveMatches(List<Match> matches);
        Task<List<int>> GetActiveMatchIds();
        Task DeactivateMatches(List<int> matchIds);
        Task SaveMatchResults(List<MatchResult> results);
        Task<List<int>> GetExistingMatchIds(List<int> matchIds);
        Task<Dictionary<string, string>> GetLatestResultValues(int matchId, List<string> keys);
    }

    public class BetCityRepository : IBetCityRepository
    {
        private readonly IDbConnection _dbConnection;

        public BetCityRepository(IConfiguration configuration)
        {
            _dbConnection = new MySqlConnection(configuration.GetConnectionString("OddsWarehouse"));
        }

        public async Task SaveLeagueGroups(List<LeagueGroup> leagueGroups)
        {
            var query = "INSERT INTO betcity_league_group (id, name, create_time, update_time) VALUES (@Id, @Name, NOW(), NOW()) ON DUPLICATE KEY UPDATE name = @Name, update_time = NOW();";
            await _dbConnection.ExecuteAsync(query, leagueGroups);
        }

        public async Task SaveLeagues(List<League> leagues)
        {
            var query = "INSERT INTO betcity_league (id, name, league_group_id, sport_type, create_time, update_time) VALUES (@Id, @Name, @LeagueGroupId, @SportId, NOW(), NOW()) ON DUPLICATE KEY UPDATE name = @Name, league_group_id = @LeagueGroupId, sport_type=@SportId, update_time = NOW();";
            await _dbConnection.ExecuteAsync(query, leagues);
        }

        public async Task SaveTeams(List<Team> teams)
        {
            var query = "INSERT INTO betcity_team (id, name, league_group_id, country, create_time, update_time) VALUES (@Id, @Name, @LeagueGroupId, @Country, NOW(), NOW()) ON DUPLICATE KEY UPDATE name = @Name, league_group_id = @LeagueGroupId, country = @Country, update_time = NOW();";
            await _dbConnection.ExecuteAsync(query, teams);
        }

        public async Task SaveMatches(List<Match> matches)
        {
            var query = "INSERT INTO `betcity_match` (id, team_1_id, team_2_id, is_active, status, kickoff_time, league_id, create_time, update_time) VALUES (@Id, @Team1Id, @Team2Id, @IsActive, @Status, @KickoffTime, @LeagueId, NOW(), NOW()) ON DUPLICATE KEY UPDATE team_1_id = @Team1Id, team_2_id = @Team2Id, is_active = @IsActive, status = @Status, kickoff_time = @KickoffTime, league_id = @LeagueId, update_time = NOW();";
            await _dbConnection.ExecuteAsync(query, matches);
        }

        public async Task<List<int>> GetActiveMatchIds()
        {
            var query = "SELECT id FROM `betcity_match` WHERE is_active = 1;";
            var result = await _dbConnection.QueryAsync<int>(query);
            return result.ToList();
        }

        public async Task DeactivateMatches(List<int> matchIds)
        {
            if (matchIds == null || !matchIds.Any())
            {
                return;
            }

            var query = "UPDATE `betcity_match` SET is_active = 0, update_time = NOW() WHERE id IN @MatchIds;";
            await _dbConnection.ExecuteAsync(query, new { MatchIds = matchIds });
        }

        public async Task SaveMatchResults(List<MatchResult> results)
        {
            if (results == null || !results.Any())
            {
                return;
            }

            // Group results by match_id for efficient querying
            var resultsByMatch = results.GroupBy(r => r.MatchId).ToList();

            var resultsToInsert = new List<MatchResult>();

            foreach (var matchGroup in resultsByMatch)
            {
                int matchId = matchGroup.Key;
                var keys = matchGroup.Select(r => r.Key).ToList();

                // Get latest values for this match
                var latestValues = await GetLatestResultValues(matchId, keys);

                // Only insert if value has changed or doesn't exist
                foreach (var result in matchGroup)
                {
                    string compositeKey = $"{matchId}_{result.Key}";

                    if (!latestValues.TryGetValue(compositeKey, out string? existingValue) ||
                        existingValue != result.Value)
                    {
                        resultsToInsert.Add(result);
                    }
                }
            }

            if (resultsToInsert.Any())
            {
                var query = @"
                    INSERT INTO `betcity_result`
                    (match_id, result_key, result_value, is_confirmed, create_time)
                    VALUES
                    (@MatchId, @Key, @Value, @IsConfirmed, NOW());";

                await _dbConnection.ExecuteAsync(query, resultsToInsert);
            }
        }

        public async Task<List<int>> GetExistingMatchIds(List<int> matchIds)
        {
            if (matchIds == null || !matchIds.Any())
            {
                return new List<int>();
            }

            var query = "SELECT id FROM `betcity_match` WHERE id IN @MatchIds;";
            var result = await _dbConnection.QueryAsync<int>(query, new { MatchIds = matchIds });
            return result.ToList();
        }

        public async Task<Dictionary<string, string>> GetLatestResultValues(int matchId, List<string> keys)
        {
            if (keys == null || !keys.Any())
            {
                return new Dictionary<string, string>();
            }

            // Get the latest value for each key by finding max(id) grouped by result_key
            var query = @"
                SELECT r1.match_id, r1.result_key, r1.result_value
                FROM `betcity_result` r1
                INNER JOIN (
                    SELECT result_key, MAX(id) as max_id
                    FROM `betcity_result`
                    WHERE match_id = @MatchId AND result_key IN @Keys
                    GROUP BY result_key
                ) r2 ON r1.id = r2.max_id;";

            var results = await _dbConnection.QueryAsync<dynamic>(
                query,
                new { MatchId = matchId, Keys = keys }
            );

            var dictionary = new Dictionary<string, string>();
            foreach (var row in results)
            {
                string compositeKey = $"{row.match_id}_{row.result_key}";
                dictionary[compositeKey] = row.result_value;
            }

            return dictionary;
        }
    }
}
