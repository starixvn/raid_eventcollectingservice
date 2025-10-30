using System.Data;
using Dapper;
using eventcollectingservice.Models.IPTV;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;

namespace eventcollectingservice.Repositories
{
    public interface IIPTVRepository
    {
        Task<int> SaveLeagues(League league);
        Task<int> SaveTeams(Team team);
        Task SaveMatches(List<GameMatch> matches);
    }

    public class IPTVRepository : IIPTVRepository
    {
        private readonly IDbConnection _dbConnection;

        public IPTVRepository(IConfiguration configuration)
        {
            _dbConnection = new MySqlConnection(configuration.GetConnectionString("IPTV"));
        }

        public async Task<int> SaveLeagues(League league)
        {
            var sql = @"
                INSERT INTO iptv_league (name, create_time, update_time)
                VALUES (@Name, NOW(), NOW())
                ON DUPLICATE KEY UPDATE
                    id = LAST_INSERT_ID(id),
                    update_time = NOW();
                SELECT LAST_INSERT_ID();
            ";

            return await _dbConnection.ExecuteScalarAsync<int>(sql, league);
        }

        public async Task<int> SaveTeams(Team team)
        {
            var sql = @"
                INSERT INTO iptv_team (name, create_time, update_time)
                VALUES (@Name, NOW(), NOW())
                ON DUPLICATE KEY UPDATE
                    id = LAST_INSERT_ID(id),
                    update_time = NOW();
                SELECT LAST_INSERT_ID();
            ";

            return await _dbConnection.ExecuteScalarAsync<int>(sql, team);
        }

        public async Task SaveMatches(List<GameMatch> matches)
        {
            var sql = @"
                INSERT INTO iptv_match (
                    team1_id,
                    team2_id,
                    kickoff_time,
                    league_id,
                    channel,
                    status,
                    is_active,
                    create_date,
                    create_time,
                    update_time
                ) VALUES (
                    @Team1Id,
                    @Team2Id,
                    @KickoffTime,
                    @LeagueId,
                    @Channel,
                    @Status,
                    @Alive,
                    CURDATE(),
                    NOW(),
                    NOW()
                )
                ON DUPLICATE KEY UPDATE
                    team1_id = VALUES(team1_id),
                    team2_id = VALUES(team2_id),
                    kickoff_time = VALUES(kickoff_time),
                    league_id = VALUES(league_id),
                    channel = VALUES(channel),
                    status = VALUES(status),
                    is_active = VALUES(is_active),
                    update_time = NOW();
            ";

            await _dbConnection.ExecuteAsync(sql, matches);
        }
    }
}
