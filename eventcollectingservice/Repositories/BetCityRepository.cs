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
    }

    public class BetCityRepository : IBetCityRepository
    {
        private readonly IDbConnection _dbConnection;

        public BetCityRepository(IConfiguration configuration)
        {
            _dbConnection = new MySqlConnection(configuration.GetConnectionString("BetCity"));
        }

        public async Task SaveLeagueGroups(List<LeagueGroup> leagueGroups)
        {
            var query = "INSERT INTO league_group (id, name, create_time, update_time) VALUES (@Id, @Name, NOW(), NOW()) ON DUPLICATE KEY UPDATE name = @Name, update_time = NOW();";
            await _dbConnection.ExecuteAsync(query, leagueGroups);
        }

        public async Task SaveLeagues(List<League> leagues)
        {
            var query = "INSERT INTO league (id, name, league_group_id, create_time, update_time) VALUES (@Id, @Name, @LeagueGroupId, NOW(), NOW()) ON DUPLICATE KEY UPDATE name = @Name, league_group_id = @LeagueGroupId, update_time = NOW();";
            await _dbConnection.ExecuteAsync(query, leagues);
        }

        public async Task SaveTeams(List<Team> teams)
        {
            var query = "INSERT INTO team (id, name, league_group_id, country, create_time, update_time) VALUES (@Id, @Name, @LeagueGroupId, @Country, NOW(), NOW()) ON DUPLICATE KEY UPDATE name = @Name, league_group_id = @LeagueGroupId, country = @Country, update_time = NOW();";
            await _dbConnection.ExecuteAsync(query, teams);
        }

        public async Task SaveMatches(List<Match> matches)
        {
            var query = "INSERT INTO `match` (id, team_1_id, team_2_id, kickoff_time, league_id, create_time, update_time) VALUES (@Id, @Team1Id, @Team2Id, @KickoffTime, @LeagueId, NOW(), NOW()) ON DUPLICATE KEY UPDATE team_1_id = @Team1Id, team_2_id = @Team2Id, kickoff_time = @KickoffTime, league_id = @LeagueId, update_time = NOW();";
            await _dbConnection.ExecuteAsync(query, matches);
        }
    }
}
