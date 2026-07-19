using Microsoft.EntityFrameworkCore;
using Trainee_Tracker.Models;

// Code-Owner: Andrej Basara

namespace Trainee_Tracker.Data.TraineeRepository
{
    public class TraineeRepository : ITraineeRepository
    {
        private readonly AppDbContext _context;

        public TraineeRepository(AppDbContext context)
        {
            _context = context;
        }

        public Trainee? FindById(int id)
        {
            return _context.Trainees.FirstOrDefault(t => t.Id == id);
        }

        public Trainee? FindByIdWithMentors(int id)
        {
            return _context.Trainees.Include(t => t.Mentors).FirstOrDefault(t => t.Id == id);
        }

        public IList<Trainee> GetAllTrainees()
        {
            return _context.Trainees.Include(t => t.Mentors).Include(t => t.Assignments).ToList();
        }

        public void Save(Trainee trainee)
        {
            _context.Trainees.Update(trainee);
            _context.SaveChanges();
        }
    }
}
