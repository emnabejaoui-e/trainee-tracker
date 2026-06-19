using Microsoft.AspNetCore.Mvc;

namespace Trainee_Tracker.Controllers;

public class MentorController : Controller
{
    /// <summary>
    ///  Assigns a Mentor to a trainee.
    /// </summary>
    /// <param name="mentorId">The numeric id of the Mentor to assign.</param>
    /// <param name="traineeId">The numeric id of the Trainee to assign.</param>
    /// <returns></returns>
    public IActionResult AssignTrainee(int mentorId, int traineeId)
    {
        return StatusCode(501, "Not implemented!");
    }
    
    /// <summary>
    /// Changes the order of lessons for a particular trainee.
    /// </summary>
    /// <param name="traineeId">The numeric id of the Trainee to change the order for.</param>
    /// <param name="order">A list with the numeric ids of Lessons in the order they should appear for trainees.</param>
    /// <returns></returns>
    public IActionResult UpdateLessonOrder(int traineeId, List<int> order)
    {
        return StatusCode(501, "Not implemented!");
    }
    
}