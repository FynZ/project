using System;
using System.Collections.Generic;
using System.Linq;
using SimpleAuthApp.Models;

namespace SimpleAuthApp.Repositories
{
    public class UserRepository : IUserRepository
    {
        private static readonly List<User> _users = new List<User>
        {
            new User
            {
                Id = 1,
                Email = "test@test.test",
                Username = "test",
                Password = "7iaw3Ur350mqGo7jwQrpkj9hiYB3Lkc/iBml1JQODbJ6wYX4oOHV+E+IvIh/1nsUNzLDBMxfqa2Ob1f1ACio/w==",
                Roles = new List<Role>
                {
                    new Role {Id = 1, Name = "User"},
                    new Role {Id = 2, Name = "Admin"},
                }
            }
        };

        public User GetUserByEmail(string email)
        {
            return _users.FirstOrDefault(x => String.Equals(x.Email, email, StringComparison.InvariantCultureIgnoreCase));
        }

        public User GetUserByUsername(string username)
        {
            return _users.FirstOrDefault(x => String.Equals(x.Username, username, StringComparison.InvariantCultureIgnoreCase));
        }

        public IEnumerable<User> GetAllUsers()
        {
            return _users;
        }

        public void CreateUser(User user)
        {
            user.Id = _users.Count;
            user.Roles = new List<Role>
            {
                new Role {Id = 1, Name = "User"}
            };

            _users.Add(user);
        }
    }
}
