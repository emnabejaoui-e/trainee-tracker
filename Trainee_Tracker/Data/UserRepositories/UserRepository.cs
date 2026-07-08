// Code Owner: Jelena Cosic
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
            var trainees = _context.Set<Trainee>().Include(t => t.Mentors).ToList<User>();
            var mentors = _context.Set<Mentor>().ToList<User>();
            return trainees.Concat(mentors).ToList();
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


        // Code Owner: Andrej Basara
        public void UpdateUser(User user, string? password = null)
        {
            if (!string.IsNullOrEmpty(password))
            {
                user.HashedPassword = password;
            }
            _context.Set<User>().Update(user);
            _context.SaveChanges();
        }

        public void CloseUser(int id)
        {
            var user = GetById(id);
            if (user != null)
            {
                user.Closed = true;
                _context.SaveChanges();
            }
        }

        public bool ValidateUserCredentials(string email, string password)
        {
            var user = GetUserByEmail(email);
            if (user == null) return false;
            return BCrypt.Net.BCrypt.Verify(password, user.HashedPassword);
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