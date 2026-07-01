using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Trainee_Tracker.Data.LessonAssignments;
using Trainee_Tracker.Models;

namespace Trainee_Tracker.Controllers;

public class LessonBoardController : Controller
{
    private readonly ILessonAssignmentRepository _lessonAssignmnetRepo;

    // Julia
    public LessonBoardController(ILessonAssignmentRepository repo)
    {
        _lessonAssignmnetRepo = repo;
    }

    // Julia
    public IActionResult LessonBoard()
    {
        var lessons = _lessonAssignmnetRepo.GetAllLessonAssignments();
        return View(lessons);
    }

    //Julia
    [HttpPost]
    public IActionResult UpdateStatus(int id, LessonAssignmentStatus newStatus){
        _lessonAssignmnetRepo.UpdateStatus(id, newStatus);
        return RedirectToAction("LessonBoard");

    }

    //Julia
    [HttpPost]
    public IActionResult RateAssignment(int id)
    {
       // _lessonAssignmnetRepo.UpdateStatus(id, LessonAssignmentStatus.Rated); -> methode gehört in FeedbackController zu CreateFeedback (zuerst merge von Julia dann von Emna)
        return RedirectToAction("CreateFeedback", "Feedback", new {id = id});
    }
}