using System.Collections.Generic;
using SimpleAuthApp.DTO;
using SimpleAuthApp.Models;

namespace SimpleAuthApp.Services
{
    public interface IUserService
    {
        JWT Authenticate(string username, string password);
        IEnumerable<User> GetAll();

        RegisterResult CreateUser(User user, string password);
    }
}
