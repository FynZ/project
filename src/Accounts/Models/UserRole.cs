using WebApi.Shared.Enumerations;

namespace Accounts.Models
{
    /// <summary>
    /// Placeholder object only because dapper has terrible support for postgres (no direct enum mapping)
    /// </summary>
    public class UserRole
    {
        public int UserId { get; set; }
        public Role Role { get; set; }
    }
}
