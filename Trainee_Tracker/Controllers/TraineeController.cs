// Code Owner: Jelena Cosic

using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Trainee_Tracker.Data.LessonAssignments;
using Trainee_Tracker.Models;
using Trainee_Tracker.Repositories;

namespace Trainee_Tracker.Controllers;

[Authorize(Roles = "Trainee")]

// Code Owner: Andrej Basara
public class TraineeController : Controller
{
    private readonly ILessonAssignmentRepository _lessonAssignmentRepo;
    private readonly IUserRepository _userRepo;

    public TraineeController(ILessonAssignmentRepository lessonAssignmentRepo, IUserRepository userRepo)
    {
        _lessonAssignmentRepo = lessonAssignmentRepo;
        _userRepo = userRepo;
    }

    public IActionResult Index()
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

        var today = DateOnly.FromDateTime(DateTime.Today);
        // Because .Net sees sunday as 0 and Monday as 1 and Saturday is 6
        var daysSinceMonday = ((int)today.DayOfWeek + 6) % 7;
        var weekStart = today.AddDays(-daysSinceMonday);
        var weekEnd = weekStart.AddDays(6);

        var weeklyAssignments = _lessonAssignmentRepo.FindByTrainee(trainee)
            .Where(a => a.ExpectedProcessingDate >= weekStart && a.ExpectedProcessingDate <= weekEnd)
            .OrderBy(a => a.Position)
            .ToList();

        ViewData["WeekStart"] = weekStart;
        return View(weeklyAssignments);
    }

    [HttpPost]
    public IActionResult UpdateStatus(int id, LessonAssignmentStatus newStatus)
    {
        _lessonAssignmentRepo.UpdateStatus(id, newStatus);
        return RedirectToAction("Index");
    }

    public IActionResult Home() => RedirectToAction("Index");
}