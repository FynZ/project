using System.Collections.Generic;
using Dapper;
using Trading.Models;
using Npgsql;

namespace Trading.Repositories
{
    public class TradingRepository : ITradingRepository
    {
        #region Queries
        private const string GET_ALL_USERS_MONSTERS = @"
            SELECT 
                tm.id                       AS Id,
                tm.name                     AS Name,
                tm.slug                     AS Slug,
                tm.min_level                AS MinLevel,
                tm.max_level                AS MaxLevel,
                tm.ankama_id                AS AnkamaId,
                tum.count                   AS Count,
                tum.search                  AS Search,
                tum.propose                 AS Propose,
                tum.user_id                 AS UserId 
            FROM 
                t_monsters                  AS tm 
            INNER JOIN 
                t_user_monsters             AS tum ON tum.monster_id = tm.id
            WHERE 
                tum.search = true
            OR
                tum.propose = true";

        private const string GET_USER_MONSTERS = @"
            SELECT 
                tm.id                       AS Id,
                tm.name                     AS Name,
                tm.slug                     AS Slug,
                tm.min_level                AS MinLevel,
                tm.max_level                AS MaxLevel,
                tm.ankama_id                AS AnkamaId,
                tum.count                   AS Count,
                tum.search                  AS Search,
                tum.propose                 AS Propose,
                tum.user_id                 AS UserId 
            FROM 
                t_monsters                  AS tm 
            INNER JOIN 
                t_user_monsters             AS tum ON tum.monster_id = tm.id
            WHERE 
                tum.user_id = @UserId";
        #endregion Queries

        private readonly string _connectionString;

        public TradingRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public IEnumerable<UserMonster> GetMonsters()
        {
            using (var con = new NpgsqlConnection(_connectionString))
            {
                con.Open();

                return con.Query<UserMonster>(GET_ALL_USERS_MONSTERS);
            }
        }

        public IEnumerable<UserMonster> GetUserMonsters(int userId)
        {
            using (var con = new NpgsqlConnection(_connectionString))
            {
                con.Open();

                return con.Query<UserMonster>(GET_USER_MONSTERS, new
                {
                    UserId = userId
                });
            }
        }
    }
}
