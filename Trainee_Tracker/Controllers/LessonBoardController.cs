// Code-Owner: Julia Sandner
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Trainee_Tracker.Data.LessonAssignments;
using Trainee_Tracker.Data.Rejections;
using Trainee_Tracker.Models;
using Trainee_Tracker.Repositories;
using Trainee_Tracker.Services;

namespace Trainee_Tracker.Controllers;

public class LessonBoardController : Controller
{
    private readonly IUserRepository _userRepo;
    private readonly ILessonAssignmentRepository _assignmentRepo;
    private readonly IAssignmentService _assignmentService;

    public LessonBoardController(IUserRepository userRepo, ILessonAssignmentRepository lessonAssignmentRepo, ILessonAssignmentRepository assignmentRepo, IAssignmentService assignmentService)
    {
       _userRepo = userRepo;
       _assignmentRepo = assignmentRepo;
        _assignmentService = assignmentService;
    }

    //Code-Owner: Julia Sandner
    /// <summary>
    /// Displays the lessonBoard for the currently logged-in trainee including any rejection history for their assignments
    /// </summary>
    /// <returns>the LessonBoard view or Unauthorized if no valid trainee is logged in</returns>
    public IActionResult LessonBoard()
    {
        var traineeIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if(traineeIdString == null)
        {
            return Unauthorized();
        }
        var traineeId = int.Parse(traineeIdString);
        var trainee = _userRepo.GetById(traineeId) as Trainee;

        if(trainee == null)
        {
            return Unauthorized();
        }

        var result = _assignmentService.GetOverview(trainee);

        ViewBag.RejectionReasons = result.RejectionHistory;
        ViewBag.AssignmentsWithHistory = result.AssignmentsWithHistory;

        return View(result.Assignments);
    }

    //Code-Owner: Julia Sandner
    /// <summary>
    /// Updates the status of a lesson assignment
    /// </summary>
    /// <param name="id"> numeric id of the assignment to update</param>
    /// <param name="newStatus"> the new status to set on the assignment</param>
    /// <returns> a redirect to the LessonBoard or Not-Found if the assignment does not exist</returns>
    [HttpPost]
    public IActionResult UpdateStatus(int id, LessonAssignmentStatus newStatus)
    {
        var currentAssignment = _assignmentRepo.GetById(id);
        if(currentAssignment == null)
        {
            return NotFound();
        }

        try
        {
            var assignment = _assignmentService.UpdateAssignmentStatus(id, newStatus);
            return RedirectToAction("LessonBoard", new {traineeId = assignment!.TraineeId});
        }
        catch(InvalidOperationException e)
        {
            TempData["Error"] = e.Message;
            return RedirectToAction("LessonBoard", new {traineeId = currentAssignment.TraineeId});
        }
    }

    //Code-Owner: Julia Sandner
    /// <summary>
    /// Redirects to the feedback-creation fo a given assignment
    /// </summary>
    /// <param name="id"> numeric id of the assignment to rate</param>
    /// <returns> a redirect to the Feedback controller's CreateFeedback action</returns>
    [HttpPost]
    public IActionResult RateAssignment(int id)
    {
        return RedirectToAction("CreateFeedback", "Feedback", new {id = id});
    }

}