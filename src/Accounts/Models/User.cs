using System.Collections.Generic;
using Accounts.Models.Enumerations;

namespace Accounts.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string UsernameUpper { get; set; }
        public string Email { get; set; }
        public string EmailUpper { get; set; }
        public string Password { get; set; }
        public Server Server { get; set; }
        public string InGameName { get; set; }
        public bool Subscribed { get; set; }
        public bool Verified { get; set; }
        public bool Banned { get; set; }
        public IEnumerable<Role> Roles { get; set; }
    }
}
