using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using SimpleAuthApp.Configuration;
using SimpleAuthApp.Configuration.Security;
using SimpleAuthApp.DTO;
using SimpleAuthApp.Models;
using SimpleAuthApp.Repositories;

namespace SimpleAuthApp.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IJwtHandler _jwtHandler;

        public UserService(IUserRepository userRepository, IJwtHandler jwtHandler)
        {
            _userRepository = userRepository;
            _jwtHandler = jwtHandler;
        }

        public JWT Authenticate(string email, string password)
        {
            var user = _userRepository.GetUserByEmail(email);

            if (user != null)
            {
                if (PasswordsMatch(password, user.Password))
                {
                    var claims = new List<Claim>();
                    claims.Add(new Claim(ClaimTypes.Name, user.Username));
                    claims.AddRange(user.Roles.Select(role => new Claim(ClaimTypes.Role, role.Name)));

                    return _jwtHandler.Create(user);
                }
            }

            return null;
        }

        public IEnumerable<User> GetAll()
        {
            return _userRepository.GetAllUsers();
        }

        public RegisterResult CreateUser(User user, string password)
        {
            user.Password = HashPassword(password);

            var result = new RegisterResult
            {
                UsernameTaken = _userRepository.GetUserByUsername(user.Username) != null,
                EmailTaken = _userRepository.GetUserByEmail(user.Email) != null
            };

            if (result.IsEligible)
            {
                _userRepository.CreateUser(user);
                result.WasCreated = true;
            }

            return result;
        }

        private static bool PasswordsMatch(string givenPassword, string storedPassword)
        {
            using (var hashBuilder = SHA512.Create())
            {
                byte[] bytePassword = hashBuilder.ComputeHash(Encoding.UTF8.GetBytes(givenPassword));
                var base64Password = Convert.ToBase64String(bytePassword);

                if (base64Password != storedPassword)
                {
                    return false;
                }
            }

            return true;
        }

        private static string HashPassword(string password)
        {
            using (var hashBuilder = SHA512.Create())
            {
                byte[] bytePassword = hashBuilder.ComputeHash(Encoding.UTF8.GetBytes(password));
                return Convert.ToBase64String(bytePassword);
            }
        }
    }
}
