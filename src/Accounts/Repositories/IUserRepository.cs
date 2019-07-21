using System;
using System.Collections.Generic;
using Accounts.Models;

namespace Accounts.Repositories
{
    public interface IUserRepository
    {
        User GetUserById(int userId);
        User GetUserByEmail(string email);
        User GetUserByUsername(string username);
        IEnumerable<User> GetAllUsers();
        int CreateUser(User user);
        void UpdateLastLoginDate(int userId, DateTime date);
        void UpdateUserProfile(User user);
        void UpdateUserProfileWithPassword(User user);
        void BanUser(int userId);
        void UnbanUser(int userId);
    }
}
