using System.Collections.Generic;
using Accounts.DTO;
using Accounts.Models;

namespace Accounts.Services
{
    public interface IUserService
    {
        JWT Authenticate(string username, string password);
        IEnumerable<User> GetAll();

        RegisterResult CreateUser(User user, string password);
    }
}
