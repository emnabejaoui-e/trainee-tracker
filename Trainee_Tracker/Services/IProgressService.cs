using Trainee_Tracker.Models;

namespace Trainee_Tracker.Services;
// Code-Owner: Nazym Beisembin
public interface IProgressService
{
    ProgressControlData CalculateProgress(
        IEnumerable<LessonAssignment> assignments,
        double daysWorked);
}