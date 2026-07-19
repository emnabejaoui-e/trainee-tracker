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

   // Code Owner: Nazym Beisembin
    private readonly IProgressService _progressService;
    private readonly IWorkingHoursSyncService _workingHoursSyncService;

    public TraineeController(
    ILessonAssignmentRepository lessonAssignmentRepo,IUserRepository userRepo, ILessonDateCalculator lessonDateCalculator, IProgressService progressService, IWorkingHoursSyncService workingHoursSyncService)
{
    _lessonAssignmentRepo = lessonAssignmentRepo;
    _userRepo = userRepo;
    _lessonDateCalculator = lessonDateCalculator;
    _progressService = progressService; // Line Owner: Nazym Beisembin
    _workingHoursSyncService = workingHoursSyncService;// Line Owner: Nazym Beisembin
}

    // Code Owner: Andrej Basara
    public IActionResult WeekPlan()
    {
        var traineeIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (traineeIdString == null)
        {
            return Unauthorized();
        }

        if (!int.TryParse(traineeIdString, out var traineeId))
        {
            return BadRequest();
        }
        var trainee = _userRepo.GetById(traineeId) as Trainee;
        if (trainee == null)
        {
            return Unauthorized();
        }

        var weeklyAssignments = _lessonDateCalculator.GetAssignmentsForCurrentWeek(trainee, out var weekStart);

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

    // Code Owner: Nazym Beisembin

    [HttpGet]
    public async Task<IActionResult> MyProgress()
    {
        ViewData["NavbarOverride"] = "Trainee";

        string? traineeIdString =
            User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (!int.TryParse(traineeIdString, out int traineeId))
        {
            return Unauthorized();
        }

        Trainee? trainee =
            _userRepo.GetById(traineeId) as Trainee;

        if (trainee == null)
        {
            return Unauthorized();
        }

        DateOnly startDate = trainee.StartingDate;
        DateOnly endDate = DateOnly.FromDateTime(DateTime.Today);

        double? daysWorked =
        await _workingHoursSyncService.GetStoredPersonDaysAsync(trainee.Id, startDate, endDate);

        List<LessonAssignment> assignments =
            _lessonAssignmentRepo.FindByTrainee(trainee);

        ProgressControlData model =
            _progressService.CalculateProgress(
                assignments,
                daysWorked ?? 0
            );

        ViewBag.TraineeName = trainee.Name;

        return View(
            "~/Views/Mentor/ProgressControl.cshtml",
            model
        );
    }
}