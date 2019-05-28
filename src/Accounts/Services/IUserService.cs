using System.Collections.Generic;
using Accounts.DTO;
using Accounts.Models;

namespace Accounts.Services
{
    public interface IUserService
    {
        Jwt Authenticate(string username, string password);

        RegisterResult CreateUser(User user, string password);
    }
}
