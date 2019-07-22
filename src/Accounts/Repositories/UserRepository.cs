using System;
using System.Collections.Generic;
using System.Linq;
using Accounts.Models;
using Dapper;
using Npgsql;
using WebApi.Shared.Enumerations;

namespace Accounts.Repositories
{
    public class UserRepository : IUserRepository
    {
        static UserRepository()
        {
            NpgsqlConnection.GlobalTypeMapper.MapEnum<Role>(typeof(Role).Name);
        }

        #region Queries
        private const string SELECT_ALL = @"
            SELECT 
                id                          AS Id, 
                username                    AS Username, 
                username_upper              AS UsernameUpper, 
                email                       AS Email, 
                email_upper                 AS EmailUpper, 
                password                    AS Password, 
                server                      AS Server, 
                in_game_name                AS InGameName, 
                subscribed                  AS Subscribed, 
                verified                    AS Verified, 
                banned                      AS Banned, 
                last_login_date             AS LastLoginDate
            FROM 
                t_users";

        private const string SELECT_BY_ID = @"
            SELECT 
                id                          AS Id, 
                username                    AS Username, 
                username_upper              AS UsernameUpper, 
                email                       AS Email, 
                email_upper                 AS EmailUpper, 
                password                    AS Password, 
                server                      AS Server, 
                in_game_name                AS InGameName, 
                subscribed                  AS Subscribed, 
                verified                    AS Verified, 
                banned                      AS Banned, 
                last_login_date             AS LastLoginDate 
            FROM 
                t_users 
            WHERE 
                id = @Id;";

        private const string SELECT_BE_USERNAME = @"
            SELECT 
                id                          AS Id, 
                username                    AS Username, 
                username_upper              AS UsernameUpper, 
                email                       AS Email, 
                email_upper                 AS EmailUpper, 
                password                    AS Password, 
                server                      AS Server, 
                in_game_name                AS InGameName, 
                subscribed                  AS Subscribed, 
                verified                    AS Verified, 
                banned                      AS Banned, 
                last_login_date             AS LastLoginDate 
            FROM 
                t_users 
            WHERE 
                username_upper = @Username;";

        private const string SELECT_BY_EMAIL = @"
            SELECT 
                id                          AS Id, 
                username                    AS Username, 
                username_upper              AS UsernameUpper, 
                email                       AS Email, 
                email_upper                 AS EmailUpper, 
                password                    AS Password, 
                server                      AS Server, 
                in_game_name                AS InGameName, 
                subscribed                  AS Subscribed, 
                verified                    AS Verified, 
                banned                      AS Banned, 
                last_login_date             AS LastLoginDate 
            FROM 
                t_users 
            WHERE 
                email_upper = @Email;";

        private const string INSERT = @"
            INSERT INTO 
                t_users
            (
                username, 
                username_upper, 
                email, 
                email_upper, 
                password,
                server,
                in_game_name,
                subscribed, 
                verified, 
                banned, 
                creation_date,
                last_login_date 
            ) 
            VALUES 
            (
                @Username, 
                @UsernameUpper, 
                @Email, 
                @EmailUpper, 
                @Password, 
                @Server::server, 
                @InGameName, 
                @Subscribed, 
                @Verified, 
                @Banned, 
                @CreationDate,
                @LastLoginDate
            )
            RETURNING id;";

        private const string SELECT_USER_ROLES = @"
            SELECT 
                tur.user_id                 AS UserId,
                tur.role::role              AS Role 
            FROM 
                t_user_roles                AS tur
            WHERE 
                tur.user_id = @Id;";

        private const string INSERT_USER_ROLE = @"
            INSERT INTO 
                t_user_roles 
            (
                user_id, 
                role
            ) 
            VALUES 
            (
                @UserId, 
                @Role::role
            );";

        private const string UPDATE_LAST_LOGIN_DATE = @"
            UPDATE
                t_users as tu
            SET
                last_login_date = @Date
            WHERE
                tu.id = @Id;";

        private const string UPDATE_USER_PROFILE = @"
            UPDATE
                t_users as tu
            SET 
                email = @Email, 
                email_upper = @EmailUpper, 
                server = @Server::server,
                in_game_name = @InGameName,
                subscribed = @Subscribed 
            WHERE
                id = @UserId";

        private const string UPDATE_USER_PROFILE_WITH_PASSWORD = @"
            UPDATE
                t_users as tu
            SET 
                email = @Email, 
                email_upper = @EmailUpper, 
                password = @Password,
                server = @Server::server,
                in_game_name = @InGameName,
                subscribed = @Subscribed 
            WHERE
                id = @UserId";

        private const string BAN_USER = @"
            UPDATE
                t_users as tu
            SET 
                banned = true
            WHERE
                id = @UserId";

        private const string UNBAN_USER = @"
            UPDATE
                t_users as tu
            SET 
                banned = false
            WHERE
                id = @UserId";
        #endregion Queries

        private readonly string _connectionString;

        public UserRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public User GetUserById(int userId)
        {
            using (var con = new NpgsqlConnection(_connectionString))
            {
                con.Open();

                var user = con.Query<User>(SELECT_BY_ID, new
                {
                    Id = userId
                }).FirstOrDefault();

                if (user != null)
                {
                    user.Roles = this.GetUserRoles(con, user.Id);
                }

                return user;
            }
        }

        public User GetUserByEmail(string email)
        {
            using (var con = new NpgsqlConnection(_connectionString))
            {
                con.Open();

                var user = con.Query<User>(SELECT_BY_EMAIL, new
                {
                    Email = email.ToUpperInvariant()
                }).FirstOrDefault();

                if (user != null)
                {
                    user.Roles = this.GetUserRoles(con, user.Id);
                }

                return user;
            }
        }

        public User GetUserByUsername(string username)
        {
            using (var con = new NpgsqlConnection(_connectionString))
            {
                con.Open();

                var user = con.Query<User>(SELECT_BE_USERNAME, new
                {
                    Username = username.ToUpperInvariant()
                }).FirstOrDefault();

                if (user != null)
                {
                    user.Roles = this.GetUserRoles(con, user.Id);
                }

                return user;
            }
        }

        public IEnumerable<User> GetAllUsers()
        {
            using (var con = new NpgsqlConnection(_connectionString))
            {
                con.Open();

                return con.Query<User>(SELECT_ALL);
            }
        }

        public int CreateUser(User user)
        {
            using (var con = new NpgsqlConnection(_connectionString))
            {
                con.Open();

                using (var transaction = con.BeginTransaction())
                {
                    // insert user
                    var insertedUserId = con.QueryFirst<int>(INSERT, new
                    {
                        Username = user.Username,
                        UsernameUpper = user.UsernameUpper,
                        Email = user.Email,
                        EmailUpper = user.EmailUpper,
                        Password = user.Password,
                        Server = user.Server.ToString(),
                        InGameName = user.InGameName,
                        Subscribed = user.Subscribed,
                        Verified = user.Verified,
                        Banned = user.Banned,
                        CreationDate = user.CreationDate,
                        LastLoginDate = user.LastLoginDate
                    }, transaction);

                    // insert roles (default is only user)
                    con.Execute(INSERT_USER_ROLE, user.Roles.Select(x => new {UserId = insertedUserId, Role = x.ToString()}), transaction);

                    transaction.Commit();

                    return insertedUserId;
                }
            }
        }

        public void UpdateLastLoginDate(int userId, DateTime date)
        {
            using (var con = new NpgsqlConnection(_connectionString))
            {
                con.Open();

                con.Execute(UPDATE_LAST_LOGIN_DATE, new
                {
                    Date = date,
                    Id = userId
                });
            }
        }

        public void UpdateUserProfile(User user)
        {
            using (var con = new NpgsqlConnection(_connectionString))
            {
                con.Open();

                con.Execute(UPDATE_USER_PROFILE, new
                {
                    UserId = user.Id,
                    Email = user.Email,
                    EmailUpper = user.EmailUpper,
                    Server = user.Server.ToString(),
                    InGameName = user.InGameName,
                    Subscribed = user.Subscribed
                });
            }
        }

        public void UpdateUserProfileWithPassword(User user)
        {
            using (var con = new NpgsqlConnection(_connectionString))
            {
                con.Open();

                con.Execute(UPDATE_USER_PROFILE_WITH_PASSWORD, new
                {
                    UserId = user.Id,
                    Email = user.Email,
                    EmailUpper = user.EmailUpper,
                    Password = user.Password,
                    Server = user.Server.ToString(),
                    InGameName = user.InGameName,
                    Subscribed = user.Subscribed
                });
            }
        }

        private IEnumerable<Role> GetUserRoles(NpgsqlConnection con, int userId)
        {
            return con.Query<UserRole>(SELECT_USER_ROLES, new
            {
                Id = userId
            }).Select(x => x.Role);
        }

        public void BanUser(int userId)
        {
            using (var con = new NpgsqlConnection(_connectionString))
            {
                con.Open();

                con.Execute(BAN_USER, new
                {
                    UserId = userId,
                });
            }
        }

        public void UnbanUser(int userId)
        {
            using (var con = new NpgsqlConnection(_connectionString))
            {
                con.Open();

                con.Execute(UNBAN_USER, new
                {
                    UserId = userId,
                });
            }
        }

        #region InMemoryImplementation
        //private static readonly List<User> _users = new List<User>
        //{
        //    new User
        //    {
        //        Id = 1,
        //        Email = "test@test.test",
        //        Username = "test",
        //        Password = "7iaw3Ur350mqGo7jwQrpkj9hiYB3Lkc/iBml1JQODbJ6wYX4oOHV+E+IvIh/1nsUNzLDBMxfqa2Ob1f1ACio/w==",
        //        Roles = new List<Role>
        //        {
        //            new Role {Id = 1, Name = "User"},
        //            new Role {Id = 2, Name = "Admin"},
        //        }
        //    }
        //};

        //public User GetUserByEmail(string email)
        //{
        //    return _users.FirstOrDefault(x => String.Equals(x.Email, email, StringComparison.InvariantCultureIgnoreCase));
        //}

        //public User GetUserByUsername(string username)
        //{
        //    return _users.FirstOrDefault(x => String.Equals(x.Username, username, StringComparison.InvariantCultureIgnoreCase));
        //}

        //public IEnumerable<User> GetAllUsers()
        //{
        //    return _users;
        //}

        //public void CreateUser(User user)
        //{
        //    user.Id = _users.Count;
        //    user.Roles = new List<Role>
        //    {
        //        new Role {Id = 1, Name = "User"}
        //    };

        //    _users.Add(user);
        //}
        #endregion InMemoryImplementation
    }
}
