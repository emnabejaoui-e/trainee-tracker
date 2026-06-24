using Trainee_Tracker.Models;

namespace Trainee_Tracker.Data.LessonAssignments;

public class StaticLessonAssignemtRepository : ILessonAssignmentRepository
{
    private static List<LessonAssignment> _data;

    static StaticLessonAssignemtRepository()
    {
        _data = new()
        {
            new LessonAssignment(1, new Lesson {Id = 11, Title = "Introduction to HTML", URL ="#", Effort = 1.0, Inactive = false, Position = 2}, 2, new DateOnly(2026, 6, 24)),
            new LessonAssignment(2, new Lesson {Id = 22, Title = "CSS Basics", URL ="#", Effort = 0.5, Inactive = false, Position = 3}, 3, new DateOnly(2026, 6, 25)),
            new LessonAssignment(3, new Lesson {Id = 33, Title = "Ruby: What extend and include do", Effort = 2.0, Inactive = false, Position = 4}, 4, new DateOnly(2026, 6, 26)),
            new LessonAssignment(4, new Lesson {Id = 44, Title = "Rules of thumb against flaky specs", URL ="#", Effort = 0.5, Inactive = false, Position = 5}, 5, new DateOnly(2026, 6, 26)),
            new LessonAssignment(5, new Lesson {Id = 55, Title = "How to use API", URL ="#", Effort = 1.5, Inactive = false, Position = 6}, 6, new DateOnly(2026, 6, 27)),
        };

        _data[4].StartAssignment();
        _data[3].FinishAssignment();
        _data[2].FinishAssignment();

    }


    public void UpdateStatus(int id, LessonAssignmentStatus newStatus)
    {
        var item = _data.FirstOrDefault(l => l.Id == id);
        if(item != null)
        {
            item.Status = newStatus;
        }
    }

    public void Delete(LessonAssignment assignment)
    {
        throw new NotImplementedException();
    }

    public List<LessonAssignment> FindByStatus(Trainee trainee, LessonAssignmentStatus status)
    {
        throw new NotImplementedException();
    }

    public List<LessonAssignment> FindByTrainee(Trainee trainee)
    {
        throw new NotImplementedException();
    }

    public void Save(LessonAssignment assignment)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<LessonAssignment> GetAllLessonAssignments()
    {
        return _data;
    }
}