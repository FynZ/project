using Accounts.DTO;

namespace Accounts.Services
{
    public interface IUserService
    {
        RegisterResult CreateUser(UserCreation user);
        AuthenticationResult Authenticate(string username, string password);
    }
}
