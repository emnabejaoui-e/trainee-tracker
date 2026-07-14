// Code Owner: Jelena Cosic

using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Trainee_Tracker.Data.LessonAssignments;
using Trainee_Tracker.Models;
using Trainee_Tracker.Repositories;
using Trainee_Tracker.Services;

namespace Trainee_Tracker.Controllers;
// Code Owner: Jelena Cosic ([Authorize])
[Authorize(Roles = "Trainee")]
public class TraineeController : Controller
{
    private readonly ILessonAssignmentRepository _lessonAssignmentRepo;
    private readonly IUserRepository _userRepo;
    private readonly ILessonDateCalculator _lessonDateCalculator;

    public TraineeController(ILessonAssignmentRepository lessonAssignmentRepo, IUserRepository userRepo, ILessonDateCalculator lessonDateCalculator)
    {
        _lessonAssignmentRepo = lessonAssignmentRepo;
        _userRepo = userRepo;
        _lessonDateCalculator = lessonDateCalculator;
    }

    // Code Owner: Andrej Basara
    public IActionResult WeekPlan()
    {
        // get the trainee string id from login claim
        var traineeIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (traineeIdString == null)
        {
            return Unauthorized();
        }

        // convert the string id in int and fetch it from db hcek if possible
        if (!int.TryParse(traineeIdString, out var traineeId))
        {
            return BadRequest();
        }
        var trainee = _userRepo.GetById(traineeId) as Trainee;
        if (trainee == null)
        {
            return Unauthorized();
        }
        
        // user service to calculate rough dates of the lesson assignment
        var allAssignments = _lessonDateCalculator.RecalculateRoughExpectedDates(trainee);

        var today = DateOnly.FromDateTime(DateTime.Today);
        // Because .Net sees sunday as 0 and Monday as 1 and Saturday is 6
        var daysSinceMonday = ((int)today.DayOfWeek + 6) % 7;
        var weekStart = today.AddDays(-daysSinceMonday);
        var weekEnd = weekStart.AddDays(6);

        // only assignemnts in current week
        var weeklyAssignments = allAssignments
            .Where(a => a.ExpectedProcessingDate >= weekStart && a.ExpectedProcessingDate <= weekEnd)
            .OrderBy(a => a.Position)
            .ToList();

        ViewData["WeekStart"] = weekStart;
        return View(weeklyAssignments);
    }

    // Code Owner: Andrej Basara
    [HttpPost]
    public IActionResult UpdateStatus(int id, LessonAssignmentStatus newStatus)
    {
        _lessonAssignmentRepo.UpdateStatus(id, newStatus);
        return RedirectToAction("WeekPlan");
    }

    public IActionResult Home() => RedirectToAction("Index");
}