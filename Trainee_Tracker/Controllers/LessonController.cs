using Microsoft.AspNetCore.Mvc;
using Trainee_Tracker.Data.LessonAssignments;
using Trainee_Tracker.Models;

namespace Trainee_Tracker.Controllers;

public class LessonController : Controller
{
    private readonly ILessonAssignmentRepository _repo;

    public LessonController(ILessonAssignmentRepository repository)
    {
        _repo = repository;
    }

    public IActionResult Index()
    {
        var lessons = _repo.GetAllLessonAssignments();
        return View(lessons);
    }
}