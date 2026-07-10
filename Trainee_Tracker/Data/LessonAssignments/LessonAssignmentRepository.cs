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

    /// <summary>
    /// Removes an assignment from the Repository 
    /// </summary>
    /// <param name="assignment">LessonAssignment that is to be deleted</param>
    public void Delete(LessonAssignment assignment)
    {
        _context.LessonAssignments.Remove(assignment);
    }

    /// <summary>
    /// Returns a list of all assignments for a specific trainee that are in the state status
    /// </summary>
    /// <param name="trainee"></param>
    /// <param name="status"></param>
    /// <returns></returns>
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

    /// <summary>
    /// Changes the state of the LessonAssignment with ID “id” to newStatus. 
    ///  If no assignment with the matching ID is found, nothing is changed.
    /// </summary>
    public void UpdateStatus(int id, LessonAssignmentStatus newStatus)
    {
        var item = _context.LessonAssignments.FirstOrDefault(l => l.Id == id);
        if(item != null)
        {
            item.Status = newStatus;
            _context.SaveChanges();
        }
    }

    /// <summary>
    /// Changes the position of the LessonAssignment with ID “id” to newPosition. 
    ///  If no assignment with the matching ID is found, nothing is changed.
    /// </summary>
    public void UpdatePosition(int id, int newPosition)
    {
        var item = _context.LessonAssignments.FirstOrDefault(l => l.Id == id);
        if(item != null)
        {
            item.Position = newPosition;
            _context.SaveChanges();
        }
    }

    public LessonAssignment? GetById(int assignmentId)
    {
        return _context.LessonAssignments
        .Include(la => la.Lesson)
        .Include(la => la.Trainee)
        .FirstOrDefault(la => la.Id == assignmentId);
                
    }

    /// <summary>
    /// Changes the order of the given LessonAssignments
    /// </summary>
    /// <param name="orderedAssignmentIds"> list of the new order of lesson Assignments (only the Assignment-Ids) </param>
    public void UpdateAssignmentPositions(List<int> orderedAssignmentIds)
    {
        for(int i = 0; i < orderedAssignmentIds.Count; i++ )
        {
            var assignment = _context.LessonAssignments.Find(orderedAssignmentIds[i]);
            if(assignment != null)
            {
                assignment.Position = i + 1;
            }       
        }
        _context.SaveChanges();
    }
}