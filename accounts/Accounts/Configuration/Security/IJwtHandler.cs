using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Accounts.Models;

namespace Accounts.Configuration.Security
{
    public interface IJwtHandler
    {
        JWT Create(User user);
    }
}
