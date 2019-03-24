using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleAuthApp.DTO
{
    public class RegisterResult
    {
        public bool EmailTaken { get; set; }
        public bool UsernameTaken { get; set; }
        public bool WasCreated { get; set; }

        public RegisterResult()
        {
            WasCreated = false;
        }

        public bool IsElligible => EmailTaken == false && UsernameTaken == false;
    }
}
