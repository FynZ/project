using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Accounts.DTO
{
    public class AuthenticationResult
    {
        public bool Authenticated { get; set; }
        public AuthResult AuthenticationOutcome { get; set; }
        public Jwt Jwt { get; set; }
    }
}
