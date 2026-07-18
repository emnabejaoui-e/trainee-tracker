using Trainee_Tracker.Models;

namespace Trainee_Tracker.Services;

// Code-Owner: Leon Paintner
public interface IMentorService
{
    /// <summary>
    /// Assigns a Mentor to a trainee.
    /// </summary>
    /// <param name="mentorId">The numeric id of the Mentor to assign.</param>
    /// <param name="traineeId">The numeric id of the Trainee to assign.</param>
    /// <returns></returns>
    void AssignTraineeToMentor(int mentorId, int traineeId);
}