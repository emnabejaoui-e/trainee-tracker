using Trainee_Tracker.Models;

namespace Trainee_Tracker.Data.Rejections;

public interface IRejectionRepository
{
    /// <summary>
    /// Get all LessonAssignments for a Trainee by TraineeId
    /// </summary>
    /// <param name="trainee"></param>
    /// <returns></returns>
    public IEnumerable<Rejection> GetRejectedByTrainee(Trainee trainee);
    public Rejection? GetById(int id);
    public Rejection Reject(int assignmentId, string reason);
}