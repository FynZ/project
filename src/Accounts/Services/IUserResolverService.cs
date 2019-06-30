using Accounts.DTO;

namespace Accounts.Services
{
    public interface IUserResolverService
    {
        SimpleUser GetUserById(int userId);
    }
}
