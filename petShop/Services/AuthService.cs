using petShop.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace petShop.Services
{
    public class AuthService : IAuthService
    {
        private readonly List<User> users;
        public AuthService(List<User> users)
        {
            this.users = users;
        }
        public User Login(string username, string password)
        {
            User user = users.FirstOrDefault(u => u.Username == username && u.Password == password);
            if (user == null)
                throw new UnauthorizedAccessException("Invalid credentials");
            Session.CurrentUser = user;
            return user;
        }

        public void Logout()
        {
            Session.CurrentUser = null;
        }
        public bool isAuthenticated()
        {
            return Session.CurrentUser != null;
        }
    }
}
