using Accounts.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Accounts.Services
{
    public interface IManagementService
    {
        IEnumerable<UserManagement> GetUsers();

        void BanUser(int userId);

        void UnbanUser(int userId);
    }
}
