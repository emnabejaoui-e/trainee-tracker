using Trainee_Tracker.Models;

namespace Trainee_Tracker.Data.LessonAssignments;

public class ListLessonAssignmentRepository : ILessonAssignmentRepository
{
    private readonly IList<LessonAssignment> _data;

    public ListLessonAssignmentRepository(IList<LessonAssignment> data)
    {
        _data = data;
    }

    public List<LessonAssignment> FindByTrainee(Trainee trainee)
    {
        return _data.Where(la => la.TraineeId == trainee.Id)
            .ToList();
    }

    public void Save(LessonAssignment assignment)
    {
        _data.Remove(assignment);
        _data.Add(assignment);
    }

    public void Delete(LessonAssignment assignment)
    {
        _data.Remove(assignment);
    }

    public IEnumerable<LessonAssignment> GetAllLessonAssignments()
    {
        return _data.AsReadOnly();
    }

    public void UpdateStatus(int id, LessonAssignmentStatus newStatus)
    {
        _data.First(la => la.Id == id).Status = newStatus;
    }

    public void UpdatePosition(int id, int newPosition)
    {
        _data.First(la => la.Id == id).Position = newPosition;
    }

    public void UpdateAssignmentPositions(List<int> orderedAssignmentIds)
    {
        for (var i = 0; i < orderedAssignmentIds.Count; i++)
        {
            _data.First(la => la.Id == orderedAssignmentIds[i]).Position = i + 1;
        }
    }

    public LessonAssignment? GetById(int assignmentId)
    {
        return _data.FirstOrDefault(la => la.Id == assignmentId, null);
    }
}