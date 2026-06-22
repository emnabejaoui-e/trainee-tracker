namespace Trainee_Tracker.Services;

public interface IMentorService
{
    /// <summary>
    /// Changes the order of lessons for a particular trainee.
    /// </summary>
    /// <param name="traineeId">The numeric id of the Trainee to change the order for.</param>
    /// <param name="order">A list with the numeric ids of Lessons in the order they should appear for trainees.</param>
    void AssignTraineeToMentor(int mentorId, int traineeId);
}