// Code-Owner: Julia Sandner
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Trainee_Tracker.Data.LessonAssignments;
using Trainee_Tracker.Data.Rejections;
using Trainee_Tracker.Models;
using Trainee_Tracker.Repositories;

namespace Trainee_Tracker.Controllers;

public class LessonBoardController : Controller
{
    private readonly ILessonAssignmentRepository _lessonAssignmentRepo;
    private readonly IUserRepository _userRepo;
    private readonly IRejectionRepository _rejectionRepo;

    public LessonBoardController(ILessonAssignmentRepository lessonAssignmentRepo, IUserRepository userRepo, IRejectionRepository rejectionRepo)
    {
        _lessonAssignmentRepo = lessonAssignmentRepo;
        _userRepo = userRepo;
        _rejectionRepo = rejectionRepo;
    }

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

        var assignments = _lessonAssignmentRepo.FindByTrainee(trainee);
        var rejections = _rejectionRepo.GetRejectedByTrainee(trainee);

        var rejectionHistory = rejections
        .GroupBy(r=> r.AssignmentId)
        .ToDictionary(g => g.Key, g => g.OrderByDescending(r=>r.RejectedAt).ToList());

        var assignmentsWithHistory = assignments
        .Where(a => rejectionHistory.ContainsKey(a.Id))
        .OrderByDescending(a => rejectionHistory[a.Id].Max(r =>r.RejectedAt))
        .ToList();

        ViewBag.RejectionReasons = rejectionHistory;
        ViewBag.AssignmentsWithHistory = assignmentsWithHistory;

        
        return View(assignments);
    }

    [HttpPost]
    public IActionResult UpdateStatus(int id, LessonAssignmentStatus newStatus)
    {
        var assignment = _lessonAssignmentRepo.GetById(id);
        if(assignment == null)
        {
            return NotFound();
        }
        _lessonAssignmentRepo.UpdateStatus(id, newStatus);
        return RedirectToAction("LessonBoard", new {traineeId = assignment.TraineeId});

    }

    [HttpPost]
    public IActionResult RateAssignment(int id)
    {
        return RedirectToAction("CreateFeedback", "Feedback", new {id = id});
    }

}