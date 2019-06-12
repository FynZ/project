using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Accounts.DTO;
using Accounts.Models;
using Accounts.Repositories;

namespace Accounts.Services
{
    public class UserResolverService : IUserResolverService
    {
        private readonly IUserRepository _userRepository;
        private readonly IDictionary<int, User> _userCache;

        private readonly object _locker;

        public UserResolverService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
            _userCache = new Dictionary<int, User>();

            _locker = new object();
        }
        public SimpleUser GetUserById(int userId)
        {
            if (_userCache.ContainsKey(userId))
            {
                var user = _userCache[userId];

                return new SimpleUser
                {
                    Id = user.Id,
                    Username = user.Username
                };
            }

            AddUserToCache(userId);

            return this.GetUserById(userId);
        }

        private void AddUserToCache(int userId)
        {
            // lock to prevent multiple add
            lock (_locker)
            {
                // check if the user was not added during the previous lock (if we could not get it right away)
                if (!_userCache.ContainsKey(userId))
                {
                    var user = _userRepository.GetUserById(userId);

                    if (user != null)
                    {
                        // add user
                        _userCache.Add(user.Id, user);
                    }
                }
            }
        }
    }
}
