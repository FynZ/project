using System.Collections.Generic;
using System.Linq;
using Dapper;
using Monsters.Models;
using Npgsql;

namespace Monsters.Repositories
{
    public class MonsterRepository : IMonsterRepository
    {
        #region Queries
        private const string INIT_USER = @"
            INSERT INTO public.t_user_monsters 
            (
                monster_id,
                count,
                search,
                propose,
                user_id
            )
            SELECT
                tm.id,
                0,
                true,
                false,
                @UserId
            FROM 
                public.t_monsters AS tm;";

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
                tum.user_id = @UserId
            ORDER BY 
                tum.monster_id;";

        private const string GET_USER_MONSTER = @"
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
                tum.user_id = @UserId 
            AND 
                tum.monster_id = @MonsterId;";

        private const string INCREMENT_MONSTER = @"
            UPDATE 
                public.t_user_monsters 
            SET 
                count = count + 1 
            WHERE 
                monster_id = @MonsterId 
            AND 
                user_id = @UserId;";

        private const string DECREMENT_MONSTER = @"
            UPDATE 
                public.t_user_monsters 
            SET 
                count = count - 1 
            WHERE 
                monster_id = @MonsterId 
            AND 
                user_id = @UserId;";

        private const string SEARCH_MONSTER = @"
            UPDATE 
                public.t_user_monsters 
            SET 
                search = true 
            WHERE 
                monster_id = @MonsterId 
            AND 
                user_id = @UserId;";

        private const string UNSEARCH_MONSTER = @"
            UPDATE 
                public.t_user_monsters 
            SET 
                search = false 
            WHERE 
                monster_id = @MonsterId 
            AND 
                user_id = @UserId;";

        private const string PROPOSE_MONSTER = @"
            UPDATE 
                public.t_user_monsters 
            SET 
                propose = true 
            WHERE 
                monster_id = @MonsterId 
            AND 
                user_id = @UserId;";

        private const string UNPROPOSE_MONSTER = @"
            UPDATE 
                public.t_user_monsters 
            SET 
                propose = false 
            WHERE 
                monster_id = @MonsterId 
            AND 
                user_id = @UserId;";
        #endregion Queries

        private readonly string _connectionString;

        public MonsterRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public bool InitUser(int userId)
        {
            using (var con = new NpgsqlConnection(_connectionString))
            {
                con.Open();

                return con.Execute(INIT_USER, new
                {
                    UserId = userId
                }) > 0;
            }
        }

        public UserMonster GetUserMonster(int monsterId, int userId)
        {
            using (var con = new NpgsqlConnection(_connectionString))
            {
                con.Open();

                return con.Query<UserMonster>(GET_USER_MONSTER, new
                {
                    MonsterId = monsterId,
                    UserId = userId
                }).FirstOrDefault();
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

        public void IncrementMonster(int monsterId, int userId)
        {
            using (var con = new NpgsqlConnection(_connectionString))
            {
                con.Open();

                con.Execute(INCREMENT_MONSTER, new
                {
                    MonsterId = monsterId,
                    UserId = userId
                });
            }
        }

        public void DecrementMonster(int monsterId, int userId)
        {
            using (var con = new NpgsqlConnection(_connectionString))
            {
                con.Open();

                con.Execute(DECREMENT_MONSTER, new
                {
                    MonsterId = monsterId,
                    UserId = userId
                });
            }
        }

        public void SearchMonster(int monsterId, int userId)
        {
            using (var con = new NpgsqlConnection(_connectionString))
            {
                con.Open();

                con.Execute(SEARCH_MONSTER, new
                {
                    MonsterId = monsterId,
                    UserId = userId
                });
            }
        }

        public void UnsearchMonster(int monsterId, int userId)
        {
            using (var con = new NpgsqlConnection(_connectionString))
            {
                con.Open();

                con.Execute(UNSEARCH_MONSTER, new
                {
                    MonsterId = monsterId,
                    UserId = userId
                });
            }
        }

        public void ProposeMonster(int monsterId, int userId)
        {
            using (var con = new NpgsqlConnection(_connectionString))
            {
                con.Open();

                con.Execute(PROPOSE_MONSTER, new
                {
                    MonsterId = monsterId,
                    UserId = userId
                });
            }
        }

        public void UnproposeMonster(int monsterId, int userId)
        {
            using (var con = new NpgsqlConnection(_connectionString))
            {
                con.Open();

                con.Execute(UNPROPOSE_MONSTER, new
                {
                    MonsterId = monsterId,
                    UserId = userId
                });
            }
        }
    }
}
