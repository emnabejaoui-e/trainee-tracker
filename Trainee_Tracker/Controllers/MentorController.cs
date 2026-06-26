using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
  using Trainee_Tracker.Models;  

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
    public IActionResult UpdateLessonOrder(int traineeId, IList<int> order)
    {
        return StatusCode(501, "Not implemented!");
    }

    [HttpGet]   // add this at the top if it isn't there

public IActionResult Fortschrittskontrolle()
{
    var model = new ProgressControlData
    {
        DaysWorked = 15,
        Finished = 12,
        Open = 8,
        Buffer = 2,
        Speed = 110,
        PredictedBuffer = 3
    };

    return View(model);
}
}