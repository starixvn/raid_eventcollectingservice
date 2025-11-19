using System.Data;
using Dapper;
using eventcollectingservice.Models.BWF;
using MySql.Data.MySqlClient;

namespace eventcollectingservice.Repositories
{
    public interface IBWFRepository
    {
        Task<int> SaveLevel(Level level);
        Task<int> SavePlayer(Player player);
        Task<int> SaveLeagueMode(LeagueMode leagueMode);
        Task SaveMatches(List<Match> matches);
    }

    public class BWFRepository : IBWFRepository
    {
        private readonly IDbConnection _dbConnection;

        public BWFRepository(IConfiguration configuration)
        {
            _dbConnection = new MySqlConnection(configuration.GetConnectionString("OddsWarehouse"));
        }

        public async Task<int> SaveLevel(Level level)
        {
            // 使用 name 作為唯一鍵來判斷重複
            var query = @"
                INSERT INTO bwf_level (name, create_time, update_time)
                VALUES (@Name, NOW(), NOW())
                ON DUPLICATE KEY UPDATE
                    id = LAST_INSERT_ID(id),
                    update_time = NOW();
                SELECT LAST_INSERT_ID() as Id
            ";

            var id = await _dbConnection.ExecuteScalarAsync<int>(query, level);
            return id;
        }

        public async Task<int> SavePlayer(Player player)
        {
            var query = @"
                INSERT INTO bwf_player (id, name, country, create_time, update_time)
                VALUES (@Id, @Name, @Country, NOW(), NOW())
                ON DUPLICATE KEY UPDATE
                    id = values(id),
                    name = values(name),
                    country = values(country),
                    update_time = NOW();
            ";

            return await _dbConnection.ExecuteAsync(query, player);
        }

        public async Task<int> SaveLeagueMode(LeagueMode leagueMode)
        {
            // 使用 league_id + mode 組合作為唯一鍵來判斷重複
            var query = @"
                INSERT INTO bwf_league_mode (league_id, name, level_id, mode, age_category, max_round, create_time, update_time)
                VALUES (@LeagueId, @Name, @LevelId, @Mode, @AgeCategory, @MaxRound, NOW(), NOW())
                ON DUPLICATE KEY UPDATE
                    id = LAST_INSERT_ID(id),
                    name = VALUES(name),
                    level_id = VALUES(level_id),
                    mode = VALUES(mode),
                    age_category = VALUES(age_category),
                    max_round = VALUES(max_round),
                    update_time = NOW();
                SELECT LAST_INSERT_ID() as Id
            ";
            var id = await _dbConnection.ExecuteScalarAsync<int>(query, leagueMode);
            return id;
        }

        public async Task SaveMatches(List<Match> matches)
        {
            var query = @"
                INSERT INTO bwf_match (
                    id,
                    kickoff_time,
                    player_home_1_id,
                    player_home_2_id,
                    player_away_1_id,
                    player_away_2_id,
                    league_mode_id,
                    court,
                    round,
                    current_round,
                    is_active,
                    status,
                    create_time,
                    update_time
                )
                VALUES (
                    @Id,
                    @KickoffTime,
                    @PlayerHome1Id,
                    @PlayerHome2Id,
                    @PlayerAway1Id,
                    @PlayerAway2Id,
                    @LeagueModeId,
                    @Court,
                    @Round,
                    @CurrentRound,
                    @IsActive,
                    @Status,
                    NOW(),
                    NOW()
                )
                ON DUPLICATE KEY UPDATE
                    id = values(id),
                    kickoff_time = VALUES(kickoff_time),
                    player_home_1_id = VALUES(player_home_1_id),
                    player_home_2_id = VALUES(player_home_2_id),
                    player_away_1_id = VALUES(player_away_1_id),
                    player_away_2_id = VALUES(player_away_2_id),
                    league_mode_id = VALUES(league_mode_id),
                    court = VALUES(court),
                    round = VALUES(round),
                    current_round = VALUES(current_round),
                    is_active = VALUES(is_active),
                    status = VALUES(status),
                    update_time = NOW();
                ";

            await _dbConnection.ExecuteAsync(query, matches);
        }
    }
}

