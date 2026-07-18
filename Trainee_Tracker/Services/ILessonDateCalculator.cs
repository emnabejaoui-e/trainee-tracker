using Trainee_Tracker.Models;

namespace Trainee_Tracker.Services;

// Code-Owner: Andrej Basara

public interface ILessonDateCalculator
{
    List<LessonAssignment> RecalculateRoughExpectedDates(Trainee trainee);
}
