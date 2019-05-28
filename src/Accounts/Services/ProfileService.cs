using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Accounts.DTO;
using Accounts.Models;
using Accounts.Repositories;

namespace Accounts.Services
{
    public class ProfileService : IProfileService
    {
        private readonly IUserRepository _userRepository;

        public ProfileService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public UserInformation GetUserInformation(int userId)
        {
            var user = _userRepository.GetUserById(userId);

            if (user != null)
            {
                return new UserInformation
                {
                    Email = user.Email,
                    Username = user.Username,
                    Server = user.Server,
                    InGameName = user.InGameName
                };
            }

            return null;
        }

        public UserProfile GetUserProfile(int userId)
        {
            var user = _userRepository.GetUserById(userId);

            if (user != null)
            {
                return new UserProfile
                {
                    Username = user.Username,
                    Server = user.Server,
                    InGameName = user.InGameName
                };
            }

            return null;
        }

        public void UpdateUserProfile(User user)
        {
            throw new NotImplementedException();
        }

        public void UpdateUserProfile(User user, string password)
        {
            throw new NotImplementedException();
        }
    }
}
