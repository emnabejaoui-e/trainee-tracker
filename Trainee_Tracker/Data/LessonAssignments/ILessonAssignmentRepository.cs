// Hauptverantwortlich: Julia

using Trainee_Tracker.Models;

namespace Trainee_Tracker.Data.LessonAssignments;

public interface ILessonAssignmentRepository
{
    public List<LessonAssignment> FindByTrainee(Trainee trainee);
    public List<LessonAssignment> FindByStatus(Trainee trainee, LessonAssignmentStatus status);
    public void Save(LessonAssignment assignment);
    public void Delete(LessonAssignment assignment);
    public IEnumerable<LessonAssignment> GetAllLessonAssignments();

}