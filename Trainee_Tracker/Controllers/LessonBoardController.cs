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

    [HttpPost]
    public IActionResult UpdateStatus(int id, LessonAssignmentStatus newStatus){
        _repo.UpdateStatus(id, newStatus);
        return RedirectToAction("LessonBoard");

    }
}