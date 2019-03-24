using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SimpleAuthApp.Models;

namespace SimpleAuthApp.Repositories
{
    public class UserRepository : IUserRepository
    {
        private static readonly List<User> _users = new List<User>
        {
            new User { Id = 1, FirstName = "Test", LastName = "User", Email = "test", Username = "test", Password = "test" }
        };

        public User GetUser(string email, string password)
        {
            return _users.FirstOrDefault(x => x.Email == email && x.Password == password);
        }

        public User GetUserByEmail(string email)
        {
            return _users.FirstOrDefault(x => x.Email == email);
        }

        public User GetUserByUsername(string username)
        {
            return _users.FirstOrDefault(x => x.Username == username);
        }

        public IEnumerable<User> GetAllUsers()
        {
            return _users;
        }

        public void CreateUser(object user)
        {
            _users.Add(new User
            {

            });
        }
    }
}
