using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Accounts.DTO;

namespace Accounts.Services
{
    public interface IUserResolverService
    {
        SimpleUser GetUserById(int userId);
    }
}
