using System.Collections.Generic;
using SimpleAuthApp.Models;

namespace SimpleAuthApp.Repositories
{
    public interface IUserRepository
    {
        User GetUserByEmail(string email);
        User GetUserByUsername(string username);
        IEnumerable<User> GetAllUsers();

        void CreateUser(User user);
    }
}
