using System.Collections.Generic;
using Trainee_Tracker.Models;

namespace Trainee_Tracker.Repositories
{
    public class FakeUserRepository : IUserRepository
    {
        private readonly List<User> _users = new()
        {
            new Trainee { Name = "Anna Müller", Email = "anna@example.com", Closed = false, StartingDate = new DateOnly(2025, 1, 15) },
            new Trainee { Name = "Ben Schmidt", Email = "ben@example.com", Closed = true,  StartingDate = new DateOnly(2024, 6, 1) },
            new Mentor  { Name = "Clara Koch",  Email = "clara@example.com", Closed = false },
            new Admin   { Name = "Admin User",  Email = "admin@example.com", Closed = false },
        };

        public IList<User> GetAllUsers() => _users;
        public void CreateTrainee(Trainee t, string pw) => _users.Add(t);
        public void CreateMentor(Mentor m, string pw) => _users.Add(m);
        public void CreateAdmin(Admin a, string pw) => _users.Add(a);
        public void UpdateUser(User u) { }
        public void CloseUser(string email) { }
        public bool ValidateUserCredentials(string email, string pw) => true;
        public User GetUserByEmail(string email) => _users.Find(u => u.Email == email)!;
    }
}
