using Accounts.DTO;

namespace Accounts.Services
{
    public interface IUserService
    {
        RegisterResult CreateUser(UserCreation user);
        Jwt Authenticate(string username, string password);
    }
}
