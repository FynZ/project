using System.Collections.Generic;
using Accounts.DTO;
using Accounts.Models;

namespace Accounts.Services
{
    public interface IUserService
    {
        User GetUser(int userId);

        IEnumerable<User> GetAll();

        Jwt Authenticate(string username, string password);

        RegisterResult CreateUser(User user, string password);
    }
}
