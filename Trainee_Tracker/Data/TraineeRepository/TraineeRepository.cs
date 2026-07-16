using Microsoft.EntityFrameworkCore;
using Trainee_Tracker.Models;

namespace Trainee_Tracker.Data.TraineeRepository
{
    public class TraineeRepository : ITraineeRepository
    {
        private readonly AppDbContext _context;

        public TraineeRepository(AppDbContext context)
        {
            _context = context;
        }

        // Code-Owner: Andrej Basara
        public Trainee? FindById(int id)
        {
            return _context.Trainees.FirstOrDefault(t => t.Id == id);
        }

        // Code-Owner: Andrej Basara
        public Trainee? FindByIdWithMentors(int id)
        {
            return _context.Trainees.Include(t => t.Mentors).FirstOrDefault(t => t.Id == id);
        }

        public IList<Trainee> GetAllTrainees()
        {
            return _context.Trainees.Include(t => t.Mentors).ToList();
        }

        public void Save(Trainee trainee)
        {
            _context.Trainees.Update(trainee);
            _context.SaveChanges();
        }
    }
}
