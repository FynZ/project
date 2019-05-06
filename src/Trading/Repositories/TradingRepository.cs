using System.Collections.Generic;
using System.Linq;
using Dapper;
using Trading.Models;
using Npgsql;
using Remotion.Linq.Clauses;

namespace Trading.Repositories
{
    public class TradingRepository : ITradingRepository
    {
        #region Queries
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
                tum.search = true
            OR
                tum.propose = true";
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

                return con.Query<UserMonster>(GET_USER_MONSTERS);
            }
        }
    }
}
