using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Accounts.DTO
{
    public enum AuthResult
    {
        Success,
        InvalidEmail,
        InvalidPassword,
        NotVerified,
        Banned
    }
}
