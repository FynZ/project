using Accounts.Models;

namespace Accounts.Configuration.Security
{
    public interface IJwtHandler
    {
        Jwt Create(User user);
    }
}
