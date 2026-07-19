// Code-Owner: Julia Sandner

using Trainee_Tracker.Models;

namespace Trainee_Tracker.Data.LessonAssignments;

public interface ILessonAssignmentRepository
{
    public List<LessonAssignment> FindByTrainee(Trainee trainee);
    public void Save(LessonAssignment assignment);
    public void Delete(LessonAssignment assignment);
    public IEnumerable<LessonAssignment> GetAllLessonAssignments();
    public void UpdateStatus(int id, LessonAssignmentStatus newStatus);
    public void UpdatePosition(int id, int newPosition);
    public void UpdateAssignmentPositions(List<int> orderedAssignmentIds);
    public LessonAssignment? GetById(int assignmentId);


}