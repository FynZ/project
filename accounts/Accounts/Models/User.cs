﻿using System.Collections.Generic;

namespace Accounts.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public bool Verified { get; set; }
        public bool Banned { get; set; }
        public IEnumerable<Role> Roles { get; set; }
    }
}
