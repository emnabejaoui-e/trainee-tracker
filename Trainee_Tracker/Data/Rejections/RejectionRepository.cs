using Microsoft.EntityFrameworkCore;
using SQLitePCL;
using Trainee_Tracker.Models;

namespace Trainee_Tracker.Data.Rejections;
// Code-Owner: Julia Sandner

public class RejectionRepository : IRejectionRepository
{
    private readonly AppDbContext _context;

    public RejectionRepository(AppDbContext context)
    {
        _context = context;
    }

    public IEnumerable<Rejection> GetRejectedByTrainee(Trainee trainee)
    {
        return _context.Rejections
        .Include(r => r.Assignment)
        .ThenInclude(a => a.Lesson)
        .Where(r => r.Assignment.TraineeId == trainee.Id)
        .OrderBy(r => r.Assignment.Position)
        .ToList();
    }

    public Rejection? GetById(int id)
    {
        return _context.Rejections
        .Include(r => r.Assignment)
        .ThenInclude(a => a.Lesson)
        .Include(r => r.Assignment)
        .ThenInclude(a => a.Trainee)
        .FirstOrDefault(r => r.Id == id);
            
    }

    public Rejection Reject(int assignmentId, string reason)
    {
        var assignment = _context.LessonAssignments.Find(assignmentId);
        if(assignment == null)
        {
            throw new InvalidOperationException($"LessonAssignmnet whit Id {assignmentId} cannot be found.");
        }

        var rejection = assignment.RejectAssignment(reason);
        _context.Rejections.Add(rejection);
        _context.SaveChanges();

        return rejection;
        
    }

    public IEnumerable<Rejection> GetRejectionsByAssignmentId(int assignmentId)
    {
        return _context.Rejections
        .Where(r => r.AssignmentId == assignmentId)
        .OrderByDescending(r => r.RejectedAt)
        .ToList();
    }

    public IDictionary<int, List<Rejection>> GetByAssignmentIds(IEnumerable<int> assignmentIds)
    {
        return _context.Rejections
        .Where(r => assignmentIds.Contains(r.AssignmentId))
        .OrderByDescending(r => r.RejectedAt)
        .GroupBy(r => r.AssignmentId)
        .ToDictionary(g => g.Key, g=>g.ToList());
    }

    public void Add(Rejection rejection)
    {
        _context.Rejections.Add(rejection);
        _context.SaveChanges();
    }
}