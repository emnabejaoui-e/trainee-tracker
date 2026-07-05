using System.Collections.Generic;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Trainee_Tracker.Data.LessonAssignments;
using Trainee_Tracker.Models;
using Trainee_Tracker.Repositories;

namespace Trainee_Tracker.Controllers;

public class LessonBoardController : Controller
{
    private readonly ILessonAssignmentRepository _lessonAssignmentRepo;
    private readonly IUserRepository _userRepo;

    // Julia
    public LessonBoardController(ILessonAssignmentRepository lessonAssignmentRepo, IUserRepository userRepo)
    {
        _lessonAssignmentRepo = lessonAssignmentRepo;
        _userRepo = userRepo;
    }

    // Julia
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
        return View(assignments);
    }

    //Julia
    [HttpPost]
    public IActionResult UpdateStatus(int id, LessonAssignmentStatus newStatus){
        _lessonAssignmentRepo.UpdateStatus(id, newStatus);
        return RedirectToAction("LessonBoard");

    }

    //Julia
    [HttpPost]
    public IActionResult RateAssignment(int id)
    {
        return RedirectToAction("CreateFeedback", "Feedback", new {id = id});
    }


    //nur zu testzwecken
    [HttpGet]
    public IActionResult ReviewPreview()
    {
        var traineeIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (traineeIdString == null)
        {
            return Unauthorized();
        }
        var traineeId = int.Parse(traineeIdString);
        var trainee = _userRepo.GetById(traineeId) as Trainee;

        if (trainee == null)
        {
            return Unauthorized();
        }

        var assignments = _lessonAssignmentRepo.FindByTrainee(trainee);
        return View("~/Views/Mentor/AssignmentOverview.cshtml", assignments); 
    }
}