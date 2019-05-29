using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Accounts.Configuration.Security;
using Accounts.DTO;
using Accounts.Helpers;
using Accounts.Models;
using Accounts.Repositories;
using Accounts.Services.HostedServices;
using Accounts.Services.HostedServices.Communication;

namespace Accounts.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IMonsterServiceCommunication _monsterServiceCommunication;
        private readonly IJwtHandler _jwtHandler;

        public UserService(IUserRepository userRepository, IHostedServiceAccessor<IMonsterServiceCommunication> monsterServiceCommunicationAccessor, IJwtHandler jwtHandler)
        {
            _userRepository = userRepository;
            _monsterServiceCommunication = monsterServiceCommunicationAccessor.Service;
            _jwtHandler = jwtHandler;
        }

        public Jwt Authenticate(string email, string password)
        {
            var user = _userRepository.GetUserByEmail(email);

            if (user != null)
            {
                if (PasswordHelper.PasswordsMatch(password, user.Password))
                {
                    var claims = new List<Claim>();
                    claims.Add(new Claim(ClaimTypes.Name, user.Username));
                    claims.AddRange(user.Roles.Select(role => new Claim("roles", role.Name)));

                    _userRepository.UpdateLastLoginDate(user.Id, DateTime.UtcNow);

                    return _jwtHandler.Create(user);
                }
            }

            return null;
        }

        public RegisterResult CreateUser(User user, string password)
        {
            user.EmailUpper = user.Email.ToUpperInvariant();
            user.UsernameUpper = user.Username.ToUpperInvariant();

            // there is no logic for account verification yet, so all account are direcly flagged as verified
            user.Verified = true;

            user.Password = PasswordHelper.HashPassword(password);

            var result = new RegisterResult
            {
                UsernameTaken = _userRepository.GetUserByUsername(user.UsernameUpper) != null,
                EmailTaken = _userRepository.GetUserByEmail(user.EmailUpper) != null
            };

            if (result.IsEligible)
            {
                int userId = _userRepository.CreateUser(user);

                result.WasCreated = true;

                _monsterServiceCommunication.UserCreated(userId);
            }

            return result;
        }
    }
}
