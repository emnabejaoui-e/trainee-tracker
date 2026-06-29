// Hauptverantwortlich: Julia
using Microsoft.EntityFrameworkCore;
using Trainee_Tracker.Models;

namespace Trainee_Tracker.Data.LessonAssignments;

public class LessonAssignmentRepository : ILessonAssignmentRepository
{   
    private readonly AppDbContext _context;

    public LessonAssignmentRepository(AppDbContext context)
    {
        _context = context;
    }



    public void Delete(LessonAssignment assignment)
    {
        _context.LessonAssignments.Remove(assignment);
    }

    public List<LessonAssignment> FindByStatus(Trainee trainee, LessonAssignmentStatus status)
    {
        return _context.LessonAssignments
                .Include(la => la.Lesson)
                .Where(la => la.TraineeId == trainee.Id && la.Status == status)
                .ToList();
    }

    public List<LessonAssignment> FindByTrainee(Trainee trainee)
    {
        return _context.LessonAssignments
                .Include(la => la.Lesson)
                .Where(la => la.TraineeId == trainee.Id)
                .ToList();
    }

    public IEnumerable<LessonAssignment> GetAllLessonAssignments()
    {
        return _context.LessonAssignments
                .Include(la => la.Lesson)
                .ToList();
    }

    /// <summary>
    /// If the ID == 0, then the assignment is not yet in the database and will be added;
    /// otherwise, the assignment will be updated. 
    /// </summary>
    public void Save(LessonAssignment assignment)
    {
        if(assignment.Id == 0)
        {
            _context.LessonAssignments.Add(assignment);
        }
        else
        {
            _context.LessonAssignments.Update(assignment);
        }

        _context.SaveChanges();
    }

    public void UpdateStatus(int id, LessonAssignmentStatus newStatus)
    {
        var item = _context.LessonAssignments.FirstOrDefault(l => l.Id == id);
        if(item != null)
        {
            item.Status = newStatus;
            _context.SaveChanges();
        }
    }
}