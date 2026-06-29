using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Trainee_Tracker.Data;
using Trainee_Tracker.Models;

namespace Trainee_Tracker.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;

        public UserRepository(AppDbContext context)
        {
            _context = context;
        }

        public IList<User> GetAllUsers()
        {
            return _context.Set<User>().ToList();
        }

        public void CreateTrainee(Trainee trainee, string password)
        {
            trainee.HashedPassword = password;
            _context.Set<Trainee>().Add(trainee);
            _context.SaveChanges();
        }

        public void CreateMentor(Mentor mentor, string password)
        {
            mentor.HashedPassword = password;
            _context.Set<Mentor>().Add(mentor);
            _context.SaveChanges();
        }

        public void CreateAdmin(Admin admin, string password)
        {
            admin.HashedPassword = password;
            _context.Set<Admin>().Add(admin);
            _context.SaveChanges();
        }

        public void UpdateUser(User user)
        {
            _context.Set<User>().Update(user);
            _context.SaveChanges();
        }

        public void CloseUser(string email)
        {
            var user = GetUserByEmail(email);
            if (user != null)
            {
                user.Closed = true;
                _context.SaveChanges();
            }
        }

        public bool ValidateUserCredentials(string email, string password)
        {
            var user = GetUserByEmail(email);
            return user != null && user.HashedPassword == password;
        }

        public User GetUserByEmail(string email)
        {
            return _context.Set<User>().FirstOrDefault(u => u.Email == email);
        }

        // Code-Owner: Andrej Basara
        public User GetById(int id)
        {
            return _context.Set<User>().FirstOrDefault(u => u.Id == id);
        }
    }
}