using System;
using System.Collections.Generic;
using System.Linq;
using Accounts.Models;
using Dapper;
using Npgsql;

namespace Accounts.Repositories
{
    public class UserRepository : IUserRepository
    {
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
                banned                      AS Banned 
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
                banned                      AS Banned 
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
                banned                      AS Banned 
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
                banned                      AS Banned 
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
                banned
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
                @Banned
            );";

        private const string SELECT_USER_ROLES = @"
            SELECT 
                tr.id                       AS Id, 
                tr.name                     AS Name 
            FROM 
                t_roles                     AS tr 
            INNER JOIN 
                t_user_roles                AS tur ON tur.role_id = tr.id 
            WHERE 
                tur.user_id = @Id;";

        private const string INSERT_USER_ROLE = @"
            INSERT INTO 
                t_user_roles 
            (
                user_id, 
                role_id
            ) 
            VALUES 
            (
                @UserId, 
                @RoleId
            );";

        private const string UPDATE_LAST_LOGIN_DATE = @"
            UPDATE
                t_users as tu
            SET
                last_login_date = @Date
            WHERE
                tu.user_id = @Id;";
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

                con.Execute(INSERT, new
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
                    Banned = user.Banned
                });

                // get inserted user for id
                var insertedUser = con.Query<User>(SELECT_BY_EMAIL, new
                {
                    Email = user.Email.ToUpperInvariant()
                }).First();

                // insert default role (User)
                this.CreateUserRole(con, insertedUser.Id, (int)Models.Enumerations.Role.User);

                return insertedUser.Id;
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

        private IEnumerable<Role> GetUserRoles(NpgsqlConnection con, int userId)
        {
            return con.Query<Role>(SELECT_USER_ROLES, new
            {
                Id = userId
            });
        }

        private void CreateUserRole(NpgsqlConnection con, int userId, int roleId)
        {
            con.Execute(INSERT_USER_ROLE, new
            {
                UserId = userId,
                RoleId = roleId
            });
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
