using petShop.Model;

namespace petShop.Services
{
    public interface IAuthService
    {
        User Login(string username, string password);
        void Logout();
        bool isAuthenticated();
    }
}
