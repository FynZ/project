using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SimpleAuthApp.DTO;
using SimpleAuthApp.Models;
using SimpleAuthApp.ViewModels;

namespace SimpleAuthApp.Services
{
    public interface IUserService
    {
        User Authenticate(string username, string password);
        IEnumerable<User> GetAll();

        RegisterResult CreateUser(User user, string password);
    }
}
