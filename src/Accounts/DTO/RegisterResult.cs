﻿namespace Accounts.DTO
{
    public class RegisterResult
    {
        public bool UsernameTaken { get; set; }
        public bool EmailTaken { get; set; }
        public bool WasCreated { get; set; }

        public RegisterResult()
        {
            WasCreated = false;
        }

        public bool IsEligible => UsernameTaken == false && EmailTaken == false;
    }
}
