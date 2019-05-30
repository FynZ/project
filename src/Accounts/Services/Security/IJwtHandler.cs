using Accounts.DTO;
using Accounts.Models;

namespace Accounts.Services.Security
{
    public interface IJwtHandler
    {
        Jwt Create(User user);
    }
}
