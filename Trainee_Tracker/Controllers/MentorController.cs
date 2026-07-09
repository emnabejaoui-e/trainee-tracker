// Code Owner: Jelena Cosic (Grundgerüst, [Authorize], Index)
using System.Diagnostics.Contracts;
using System.Net.Mime;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Trainee_Tracker.Data.LessonAssignments;
using Trainee_Tracker.Data.MentorRepository;
using Trainee_Tracker.Models;
using Trainee_Tracker.Services;

namespace Trainee_Tracker.Controllers;

[Authorize(Roles = "Mentor, Admin")]
    public class MentorController : Controller
{
    private readonly IMentorRepository _mentorRepo;
    private readonly WorkingHoursService _workingHoursService;
    private readonly IProgressService _progressService;
    private readonly ILessonAssignmentRepository _lessonAssignmentRepo;

    public MentorController(
        IMentorRepository mentorRepo,
        WorkingHoursService workingHoursService,
        IProgressService progressService,
        ILessonAssignmentRepository lessonAssignmentRepo)
    {
        _mentorRepo = mentorRepo;
        _workingHoursService = workingHoursService;
        _progressService = progressService;
        _lessonAssignmentRepo = lessonAssignmentRepo;
    }
    // Code-Owner: Jelena Cosic
    // GET: /Mentor/Index
    /// <summary>
    /// Displays the Mentor dashboard.
    /// Only accessible by users with the Mentor or Admin role.
    /// </summary>
    /// <returns>The Mentor index view.</returns>
    public IActionResult Index()
    {
        ViewData["NavbarOverride"] = "Mentor";
        return View();
    }

    // Code-Owner: Leon
    /// <summary>
    /// Assigns a Mentor to a trainee.
    /// </summary>
    /// <param name="mentorId">The numeric id of the Mentor to assign.</param>
    /// <param name="traineeId">The numeric id of the Trainee to assign.</param>
    /// <returns>501 Not Implemented status.</returns>
    public IActionResult AssignTrainee(int mentorId, int traineeId)
    {
        return StatusCode(501, "Not implemented!");
    }

    // Code-Owner: Leon
    /// <summary>
    /// Changes the order of lessons for a particular trainee.
    /// </summary>
    /// <param name="traineeId">The numeric id of the Trainee to change the order for.</param>
    /// <param name="order">A list with the numeric ids of Lessons in the order they should appear for trainees.</param>
    /// <returns>501 Not Implemented status.</returns>
    public IActionResult UpdateLessonOrder(int traineeId, IList<int> order)
    {
        return StatusCode(501, "Not implemented!");
    }

    // Code-Owner: Leon
    public IActionResult MyTrainees()
    {
        string? mentorId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        Contract.Assert(mentorId != null, "user must be logged in");

        Mentor? currentUser = _mentorRepo.GetMentorById(int.Parse(mentorId));
        if (currentUser == null)
        {
            return Unauthorized();
        }
        
        return View(currentUser.AssignedTrainees);
    }

    // Code-Owner: Leon
    // GET: /Mentor/Fortschrittskontrolle

   [HttpGet]
    public async Task<IActionResult> Fortschrittskontrolle(int traineeId)
    {
        ViewData["NavbarOverride"] = "Mentor";

        string? mentorId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (mentorId == null)
        {
            return Unauthorized();
        }

        Mentor? mentor = _mentorRepo.GetMentorById(int.Parse(mentorId));
        if (mentor == null)
        {
            return Unauthorized();
        }

        Trainee? trainee = mentor.AssignedTrainees
            .FirstOrDefault(t => t.Id == traineeId);

        if (trainee == null)
        {
            return Unauthorized();
        }

        DateOnly startDate = trainee.StartingDate;
        DateOnly endDate = DateOnly.FromDateTime(DateTime.Today);

        double? daysWorked = await _workingHoursService.GetWorkedPersonDaysAsync(
            trainee.Email,
            startDate,
            endDate
        );

        List<LessonAssignment> assignments =
            _lessonAssignmentRepo.FindByTrainee(trainee);

        ProgressControlData model = _progressService.CalculateProgress(
            assignments,
            daysWorked ?? 0
        );

        return View(model);
    }

    [HttpGet]
    public IActionResult ImportCurriculum()
    {
        ViewData["NavbarOverride"] = "Mentor";

        var curriculumNames = new List<string>
        {
            "makandra Curriculum",
            "makandra DevOps Curriculum"
        };

        ViewBag.curriculumNames = curriculumNames;
        return View(null);
    }

    [HttpPost]
    public IActionResult ImportCurriculum(string curriculumName, IFormFile file)
    {
        ViewData["NavbarOverride"] = "Mentor";

        if (file == null)
        {
            ModelState.AddModelError("FileName", "No file was selected.");
        }
        else
        {
            ContentType fileContentType = new ContentType(file.ContentType);
            if (fileContentType.MediaType != "application/json")
            {
                ModelState.AddModelError("FileName", "This file is not a JSON file.");
            }
        }

        if (curriculumName.IsWhiteSpace())
        {
            ModelState.AddModelError("FileName", "No curriculum was selected.");
        }

        if (ModelState.IsValid)
        {
            return RedirectToAction("Index", "Mentor");
        }

        var curriculumNames = new List<string>
        {
            "makandra Curriculum",
            "makandra DevOps Curriculum"
        };

        ViewBag.curriculumNames = curriculumNames;
        return View(file);
    }
}