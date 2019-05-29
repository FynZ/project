using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Accounts.DTO;
using Accounts.Models;

namespace Accounts.Services
{
    public interface IProfileService
    {
        UserInformation GetUserInformation(int userId);

        UserProfile GetUserProfile(int userId);

        bool UpdateUserProfile(User user);

        bool UpdateUserProfile(User user, string password);
    }
}
