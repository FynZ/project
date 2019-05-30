using Accounts.DTO;
using Accounts.Helpers;
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
                    InGameName = user.InGameName,
                    LastLoginDate = user.LastLoginDate
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
                    InGameName = user.InGameName,
                    LastLoginDate = user.LastLoginDate
                };
            }

            return null;
        }

        public bool UpdateUserProfile(User user)
        {
            user.EmailUpper = user.Email.ToUpperInvariant();

            // check if the to update email is already taken
            var sameEMail = _userRepository.GetUserByEmail(user.EmailUpper);

            // if no match or we match the same user, it's good to go
            if (sameEMail is null || user.Id == sameEMail.Id)
            {
                _userRepository.UpdateUserProfile(user);

                return true;
            }

            return false;
        }

        public bool UpdateUserProfile(User user, string password)
        {
            user.EmailUpper = user.Email.ToUpperInvariant();
            user.Password = PasswordHelper.HashPassword(password);

            // check if the to update email is already taken
            var sameEMail = _userRepository.GetUserByEmail(user.EmailUpper);

            // if no match or we match the same user, it's good to go
            if (sameEMail is null || user.Id == sameEMail.Id)
            {
                _userRepository.UpdateUserProfileWithPassword(user);

                return true;
            }

            return false;
        }
    }
}
