using System.Collections.Generic;
using Accounts.Models;

namespace Accounts.Repositories
{
    public interface IUserRepository
    {
        User GetUserByEmail(string email);
        User GetUserByUsername(string username);
        IEnumerable<User> GetAllUsers();
        int CreateUser(User user);
    }
}
