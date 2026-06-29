using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Trainee_Tracker.Data.LessonAssignments;
using Trainee_Tracker.Models;

namespace Trainee_Tracker.Controllers;

public class LessonBoardController : Controller
{
    private readonly ILessonAssignmentRepository _repo;

    // Julia
    public LessonBoardController(ILessonAssignmentRepository repo)
    {
        _repo = repo;
    }

    // Julia
    public IActionResult LessonBoard()
    {
        var lessons = _repo.GetAllLessonAssignments();
        return View(lessons);
    }

    //Julia
    [HttpPost]
    public IActionResult UpdateStatus(int id, LessonAssignmentStatus newStatus){
        _repo.UpdateStatus(id, newStatus);
        return RedirectToAction("LessonBoard");

    }

    //Julia
    [HttpPost]
    public IActionResult RateAssignment(int id)
    {
        _repo.UpdateStatus(id, LessonAssignmentStatus.Rated);
        return RedirectToAction("CreateFeedback", "Feedback", new {id = id});
    }
}