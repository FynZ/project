using Accounts.Models.Enumerations;

namespace Accounts.DTO
{
    public class UserCreation
    {
        public string Email { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public Server Server { get; set; }
        public string InGameName { get; set; }
        public bool Subscribed { get; set; }
    }
}
