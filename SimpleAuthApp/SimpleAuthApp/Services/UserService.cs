using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using SimpleAuthApp.DTO;
using SimpleAuthApp.Models;
using SimpleAuthApp.Repositories;
using SimpleAuthApp.ViewModels;

namespace SimpleAuthApp.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly KeyManager _keyManager;

        public UserService(IUserRepository userRepository, KeyManager keyManager)
        {
            _userRepository = userRepository;
            _keyManager = keyManager;
        }

        // move thhis shit elsewhere
        public User Authenticate(string username, string password)
        {
            var user = _userRepository.GetUser(username, password);

            // return null if user not found
            if (user == null)
                return null;

            // authentication successful so generate jwt token
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.Id.ToString())
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(_keyManager.Key), _keyManager.Algorithm)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            user.Token = tokenHandler.WriteToken(token);

            // remove password before returning
            user.Password = null;

            return user;
        }

        public IEnumerable<User> GetAll()
        {
            return _userRepository.GetAllUsers();
        }

        public RegisterResult CreateUser(User user, string password)
        {
            user.Password = password;

            var result = new RegisterResult()
            {
                EmailTaken = _userRepository.GetUserByUsername(user.Username) != null,
                UsernameTaken = _userRepository.GetUserByUsername(user.Username) != null
            };

            if (result.IsElligible)
            {
                _userRepository.CreateUser(user);
                result.WasCreated = true;

                return result;
            }

            return result;
        }
    }
}
