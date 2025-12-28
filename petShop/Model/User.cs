using System;

namespace petShop.Model
{
    public class User
    {
        public Guid Id { get; private set; }
        public string Username { get; private set; }
        public string Password { get; private set; }
        public string Name { get; private set; }
        public string Surname { get; private set; }
        public Role Role { get; private set; }

        public User(string username, string password, string name, string surname, Role role)
        {
            Id = Guid.NewGuid();
            Username = username;
            Password = password;
            Name = name;
            Surname = surname;
            Role = role;
        }
    }
}
