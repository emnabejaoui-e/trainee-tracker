using Trainee_Tracker.Models;

namespace Trainee_Tracker.Data.LessonAssignments;

public class FakeLessonAssignmentRepository : ILessonAssignmentRepository
{
    private readonly IList<LessonAssignment> _data = new List<LessonAssignment>();

    public List<LessonAssignment> FindByTrainee(Trainee trainee)
    {
        return _data.Where(la => la.TraineeId == trainee.Id).ToList();
    }

    public List<LessonAssignment> FindByStatus(Trainee trainee, LessonAssignmentStatus status)
    {
        return _data.Where(la => la.TraineeId == trainee.Id && la.Status == status).ToList();
    }

    public List<LessonAssignment> FindByLesson(Lesson lesson)
    {
        return _data.Where(la => la.Lesson.Equals(lesson)).ToList();
    }

    public void Save(LessonAssignment assignment)
    {
        if (assignment.Id == 0)
        {
            _data.Add(assignment);
        }
        else
        {
            var existing = _data.FirstOrDefault(la => la.Id == assignment.Id);
            if (existing != null)
            {
                _data.Remove(existing);
                _data.Add(assignment);
            }
        }
    }

    public void Delete(LessonAssignment assignment)
    {
        _data.Remove(assignment);
    }

    public IEnumerable<LessonAssignment> GetAllLessonAssignments()
    {
        return _data.ToList();
    }

    public void UpdateStatus(int id, LessonAssignmentStatus newStatus)
    {
        var item = _data.FirstOrDefault(la => la.Id == id);
        if (item != null)
        {
            item.Status = newStatus;
        }
    }

    public void UpdatePosition(int id, int newPosition)
    {
        var item = _data.FirstOrDefault(la => la.Id == id);
        if (item != null)
        {
            item.Position = newPosition;
        }
    }

    public void UpdateAssignmentPositions(List<int> orderedAssignmentIds)
    {
        for (int i = 0; i < orderedAssignmentIds.Count; i++)
        {
            var assignment = _data.FirstOrDefault(la => la.Id == orderedAssignmentIds[i]);
            if (assignment != null)
            {
                assignment.Position = i + 1;
            }
        }
    }

    public LessonAssignment? GetById(int assignmentId)
    {
        return _data.FirstOrDefault(la => la.Id == assignmentId);
    }
}