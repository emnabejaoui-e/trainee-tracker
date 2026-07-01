using System.Linq;
using Microsoft.EntityFrameworkCore;
using Trainee_Tracker.Models;

namespace Trainee_Tracker.Data.MentorRepository
{
    public class MentorRepository : IMentorRepository
    {
        private readonly AppDbContext _context;

        public MentorRepository(AppDbContext context)
        {
            _context = context;
        }

        // Code Owner: Andrej Basara
        public Mentor? GetMentorById(int id)
        {
            return _context.Set<Mentor>()
                .Include(m => m.AssignedTrainees)
                .FirstOrDefault(u => u.Id == id);
        }

        // Code Owner: Andrej Basara
        public void UpdateMentor(Mentor mentor)
            {
                _context.Set<User>().Update(mentor);
                _context.SaveChanges();
            }
    }
}