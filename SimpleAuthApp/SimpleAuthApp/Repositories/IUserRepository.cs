﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SimpleAuthApp.Models;

namespace SimpleAuthApp.Repositories
{
    public interface IUserRepository
    {
        User GetUser(string email, string password);
        User GetUserByEmail(string email);
        User GetUserByUsername(string username);
        IEnumerable<User> GetAllUsers();

        void CreateUser(object user);
    }
}
