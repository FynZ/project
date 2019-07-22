using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Accounts.DTO;
using Accounts.Helpers;
using Accounts.Models;
using Accounts.Repositories;
using Accounts.Services.HostedServices;
using Accounts.Services.HostedServices.Communication;
using Accounts.Services.Security;
using WebApi.Shared.Enumerations;

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

        public RegisterResult CreateUser(UserCreation userCreation)
        {
            var user = new User
            {
                Username = userCreation.Username,
                UsernameUpper = userCreation.Username.ToUpperInvariant(),
                Email = userCreation.Email,
                EmailUpper = userCreation.Email.ToUpperInvariant(),
                Password = PasswordHelper.HashPassword(userCreation.Password),
                Server = userCreation.Server,
                InGameName = userCreation.InGameName,
                Subscribed = userCreation.Subscribed,
                Verified = true, // there is no logic for account verification yet, so all account are directly flagged as verified
                Banned = false,
                CreationDate = DateTime.UtcNow,
                LastLoginDate = DateTime.UtcNow,
                Roles = new List<Role> { Role.User }
            };

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

        public AuthenticationResult Authenticate(string email, string password)
        {
            var user = _userRepository.GetUserByEmail(email);

            if (user != null)
            {
                if (PasswordHelper.PasswordsMatch(password, user.Password))
                {
                    if (user.Verified == false)
                    {
                        return new AuthenticationResult
                        {
                            Authenticated = true,
                            AuthenticationOutcome = AuthResult.NotVerified,
                            Jwt = null
                        };
                    }
                    else if (user.Banned == true)
                    {
                        return new AuthenticationResult
                        {
                            Authenticated = true,
                            AuthenticationOutcome = AuthResult.Banned,
                            Jwt = null
                        };
                    }
                    else
                    {
                        var claims = new List<Claim>();
                        claims.Add(new Claim(ClaimTypes.Name, user.Username));
                        claims.AddRange(user.Roles.Select(role => new Claim("roles", role.ToString())));

                        _userRepository.UpdateLastLoginDate(user.Id, DateTime.UtcNow);

                        var token = _jwtHandler.Create(user);

                        return new AuthenticationResult
                        {
                            Authenticated = true,
                            AuthenticationOutcome = AuthResult.Success,
                            Jwt = token
                        };
                    }
                }
                else
                {
                    return new AuthenticationResult
                    {
                        Authenticated = false,
                        AuthenticationOutcome = AuthResult.InvalidPassword,
                        Jwt = null
                    };
                }
            }
            else
            {
                return new AuthenticationResult
                {
                    Authenticated = false,
                    AuthenticationOutcome = AuthResult.InvalidEmail,
                    Jwt = null
                };
            }
        }
    }
}
