using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SimpleAuthApp.Models;

namespace SimpleAuthApp.Configuration.Security
{
    public interface IJwtHandler
    {
        JWT Create(User user);
    }
}
