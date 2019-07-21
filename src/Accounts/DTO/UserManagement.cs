using Accounts.Models.Enumerations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Shared.Enumerations;

namespace Accounts.DTO
{
    public class UserManagement
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public Server Server { get; set; }
        public string InGameName { get; set; }
        public bool Subscribed { get; set; }
        public bool Verified { get; set; }
        public bool Banned { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime LastLoginDate { get; set; }
    }
}
