using Microsoft.EntityFrameworkCore;
using Trainee_Tracker.Models;

namespace Trainee_Tracker.Data.LessonFeedbacks;

public class LessonFeedbackRepository : ILessonFeedbackRepository
{
    private readonly AppDbContext _context;

    public LessonFeedbackRepository(AppDbContext context)
    {
        _context = context;
    }

    public List<LessonFeedback> FindByTrainee(Trainee trainee, DateTime from, DateTime until)
    {
        return _context.LessonFeedbacks
            .Where(feedback => feedback.TraineeId == trainee.Id
                               && feedback.CreatedAt >= from
                               && feedback.CreatedAt <= until)
            .ToList();
    }

    public List<LessonFeedback> FindByMentor(Mentor mentor, DateTime from, DateTime until)
    {
        return _context.LessonFeedbacks
            .Where(feedback => feedback.MentorId == mentor.Id
                               && feedback.CreatedAt >= from
                               && feedback.CreatedAt <= until)
            .ToList();
    }

    public List<LessonFeedback> FindByTimeSpan(DateTime from, DateTime until)
    {
        return _context.LessonFeedbacks
            .Where(feedback => feedback.CreatedAt >= from
                               && feedback.CreatedAt <= until.AddDays(1))
            .ToList();
    }

    public void Save(LessonFeedback feedback)
    {
        _context.LessonFeedbacks.Add(feedback);
        _context.SaveChanges();
    }

    public void Delete(LessonFeedback feedback)
    {
        _context.LessonFeedbacks.Remove(feedback);
        _context.SaveChanges();
    }
}