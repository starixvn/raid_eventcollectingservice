using System.Data;
using Dapper;
using eventcollectingservice.Models.Futbol24;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;

namespace eventcollectingservice.Repositories
{
    public interface IFutbol24Repository
    {
        Task SaveCategories(List<Category> categories);
        Task SaveLeagues(List<League> leagues);
        Task SaveTeams(List<Team> teams);
        Task SaveMatches(List<GameMatch> matches);
    }

    public class Futbol24Repository : IFutbol24Repository
    {
        private readonly IDbConnection _dbConnection;

        public Futbol24Repository(IConfiguration configuration)
        {
            _dbConnection = new MySqlConnection(configuration.GetConnectionString("Futbol24"));
        }

        public async Task SaveCategories(List<Category> categories)
        {
            var query = "INSERT INTO category (id, name, create_time, update_time) VALUES (@Id, @Name, NOW(), NOW()) ON DUPLICATE KEY UPDATE name = @Name, update_time = NOW();";
            await _dbConnection.ExecuteAsync(query, categories);
        }

        public async Task SaveLeagues(List<League> leagues)
        {
            var query = "INSERT INTO league (id, name, category_id, parent_league_id, create_time, update_time) VALUES (@Id, @Name, @CategoryId, @ParentLeagueId, NOW(), NOW()) ON DUPLICATE KEY UPDATE name = @Name, category_id = @CategoryId, parent_league_id = @ParentLeagueId, update_time = NOW();";
            await _dbConnection.ExecuteAsync(query, leagues);
        }

        public async Task SaveTeams(List<Team> teams)
        {
            var query = "INSERT INTO team (id, name, league_id, create_time, update_time) VALUES (@Id, @Name, @LeagueId, NOW(), NOW()) ON DUPLICATE KEY UPDATE name = @Name, league_id = @LeagueId, update_time = NOW();";
            await _dbConnection.ExecuteAsync(query, teams);
        }

        public async Task SaveMatches(List<GameMatch> matches)
        {
            var query = "INSERT INTO `match` (id, team_1_id, team_2_id, kickoff_time, league_id, create_time, update_time) VALUES (@Id, @Team1Id, @Team2Id, @KickoffTime, @LeagueId, NOW(), NOW()) ON DUPLICATE KEY UPDATE team_1_id = @Team1Id, team_2_id = @Team2Id, kickoff_time = @KickoffTime, league_id = @LeagueId, update_time = NOW();";
            await _dbConnection.ExecuteAsync(query, matches);
        }
    }
}
