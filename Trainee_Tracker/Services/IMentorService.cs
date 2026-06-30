using Trainee_Tracker.Models;

namespace Trainee_Tracker.Services;

/// <author>Leon</author>
public interface IMentorService
{
    /// <summary>
    /// Assigns a Mentor to a trainee.
    /// </summary>
    /// <param name="mentorId">The numeric id of the Mentor to assign.</param>
    /// <param name="traineeId">The numeric id of the Trainee to assign.</param>
    /// <returns></returns>
    void AssignTraineeToMentor(int mentorId, int traineeId);
    
    /// <summary>
    /// Changes the order of lessons for a particular trainee.
    /// </summary>
    /// <param name="traineeId">The numeric id of the Trainee to change the order for.</param>
    /// <param name="order">A list with the numeric ids of Lessons in the order they should appear for trainees.</param>
    void UpdateLessonOrder(int traineeId, IList<int> order);

    /// <summary>
    /// Imports 
    /// </summary>
    /// <param name="curriculumName"></param>
    /// <param name="curriculum"></param>
    void ImportCurriculum(string curriculumName, IList<Lesson> curriculum);
}