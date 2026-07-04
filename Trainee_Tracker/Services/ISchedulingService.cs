using Trainee_Tracker.Models;

namespace Trainee_Tracker.Services;

public interface ISchedulingService
{
    /// <summary>
    /// Changes the order of lessons for a particular trainee.
    /// </summary>
    /// <param name="traineeId">The numeric id of the Trainee to change the order for.</param>
    /// <param name="order">A list with the numeric ids of Lessons in the order they should appear for trainees.</param>
    void UpdateLessonOrder(int traineeId, IList<int> order);
}