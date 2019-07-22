using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Accounts.DTO;
using Accounts.Repositories;

namespace Accounts.Services
{
    public class ManagementService : IManagementService
   {
        private readonly IUserRepository _userRepository;

        public ManagementService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public IEnumerable<UserManagement> GetUsers()
        {
            var users = _userRepository.GetAllUsers();

            return users.Select(x => new UserManagement
            {
                Id = x.Id,
                Username = x.Username,
                Email = x.Email,
                Server = x.Server,
                InGameName = x.InGameName,
                Subscribed = x.Subscribed,
                Verified = x.Verified,
                Banned = x.Banned,
                CreationDate = x.CreationDate,
                LastLoginDate = x.LastLoginDate
            });
        }

        public void BanUser(int userId)
        {
            _userRepository.BanUser(userId);
        }

        public void UnbanUser(int userId)
        {
            _userRepository.UnbanUser(userId);
        }
    }
}
