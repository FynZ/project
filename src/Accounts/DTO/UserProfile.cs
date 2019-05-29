﻿using Accounts.Models.Enumerations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Accounts.DTO
{
    public class UserProfile
    {
        public string Username { get; set; }
        public Server Server { get; set; }
        public string InGameName { get; set; }
        public DateTime LastLoginDate { get; set; }
    }
}
