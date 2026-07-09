using Trainee_Tracker.Models;

namespace Trainee_Tracker.Services;

public interface IProgressService
{
    ProgressControlData CalculateProgress(
        IEnumerable<LessonAssignment> assignments,
        double daysWorked);
}