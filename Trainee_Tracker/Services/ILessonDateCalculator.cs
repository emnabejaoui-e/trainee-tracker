using Trainee_Tracker.Models;

namespace Trainee_Tracker.Services;

public interface ILessonDateCalculator
{
    List<LessonAssignment> RecalculateRoughExpectedDates(Trainee trainee);
    List<LessonAssignment> GetAssignmentsForCurrentWeek(Trainee trainee, out DateOnly weekStart);
}
