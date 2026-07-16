using System.Diagnostics.Contracts;
using System.Net.Mime;
using System.Security.Claims;
using System.Text.Json;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Trainee_Tracker.Data.LessonAssignments;
using Trainee_Tracker.Data.Curriculums;
using Trainee_Tracker.Data.MentorRepository;
using Trainee_Tracker.Data.Rejections;
using Trainee_Tracker.Data.TraineeRepository;
using Trainee_Tracker.Models;
using Trainee_Tracker.Repositories;
using Trainee_Tracker.Services;

namespace Trainee_Tracker.Controllers;
// Code Owner: Jelena Cosic ([Authorize])
[Authorize(Roles = "Mentor, Admin")]
public class MentorController : Controller
{
    private readonly ICurriculumRepository _curriculumRepo;
    private readonly ICurriculumService _curriculumService;
    private readonly IMentorRepository _mentorRepo;
    private readonly IUserRepository _userRepo;
    private readonly ILessonAssignmentRepository _assignmentRepo;
    private readonly IRejectionRepository _rejectionRepo;
    private readonly ITraineeRepository _traineeRepo;
    private readonly WorkingHoursService _workingHoursService;
    private readonly IProgressService _progressService;

    public MentorController(
        IMentorRepository mentorRepo,
        IUserRepository userRepo,
        ILessonAssignmentRepository assignmentRepo,
        IRejectionRepository rejectionRepo,
        ITraineeRepository traineeRepo,
        WorkingHoursService workingHoursService,
        IProgressService progressService,
        ICurriculumRepository curriculumRepo,
        ICurriculumService curriculumService 
        )
    {
        _mentorRepo = mentorRepo;
        _curriculumRepo = curriculumRepo;
        _userRepo = userRepo;
        _assignmentRepo = assignmentRepo;
        _rejectionRepo = rejectionRepo;
        _traineeRepo = traineeRepo;
        _workingHoursService = workingHoursService;
        _progressService = progressService;
        _curriculumRepo = curriculumRepo;
        _curriculumService = curriculumService;
    }
    
    // Code-Owner: Jelena Cosic
    // GET: /Mentor/Index
    /// <summary>
    /// Displays the Mentor dashboard.
    /// Only accessible by users with the Mentor or Admin role.
    /// </summary>
    /// <returns>The Mentor index view.</returns>
    public IActionResult Index() => RedirectToAction("MyTrainees");

    // Code-Owner: Leon Paintner
    public IActionResult MyTrainees()
    {
        string? mentorId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        Contract.Assert(mentorId != null, "user must be logged in");

        Mentor? currentUser = _mentorRepo.GetMentorById(int.Parse(mentorId));
        if (currentUser == null)
        {
            return Unauthorized();
        }
        
        ViewBag.isAdmin = "Admin".Equals(User.FindFirstValue(ClaimTypes.Role));
        if (ViewBag.isAdmin)
        {
            ViewBag.allTrainees = _traineeRepo.GetAllTrainees()
                .Where(t => !t.Closed);
        }
        
        return View(currentUser.AssignedTrainees.Where(t => !t.Closed));
    }

    // Code-Owner: Nazym Beisembin
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

        Trainee? trainee;

        if (User.IsInRole("Admin"))
        {
            trainee = _userRepo.GetById(traineeId) as Trainee;
        }
        else
        {
            Mentor? mentor = _mentorRepo.GetMentorById(int.Parse(mentorId));

            if (mentor == null)
            {
                return Unauthorized();
            }

            trainee = mentor.AssignedTrainees
                .FirstOrDefault(t => t.Id == traineeId && !t.Closed);
        }

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
            _assignmentRepo.FindByTrainee(trainee);

        ProgressControlData model = _progressService.CalculateProgress(
            assignments,
            daysWorked ?? 0
        );

        ViewBag.TraineeName = trainee.Name;

        return View(model);
    }

    // Code-Owner: Leon Paintner
    // GET: /Mentor/ImportCurriculum
    [HttpGet]
    public IActionResult ImportCurriculum()
    {
        ViewData["NavbarOverride"] = "Mentor";
        ViewBag.curriculumNames = _curriculumRepo.GetAllCurriculums().Select(c => c.Title);
        return View(null);
    }

    // Code-Owner: Leon Paintner
    // POST: /Mentor/ImportCurriculum
    [HttpPost]
    public IActionResult ImportCurriculum(string curriculumName, IFormFile file)
    {
        ViewData["NavbarOverride"] = "Mentor";
        if (file == null)
            ModelState.AddModelError("FileName", "No file was selected.");
        else
        {
            ContentType fileContentType = new ContentType(file.ContentType);
            if (fileContentType.MediaType != "application/json")
                ModelState.AddModelError("FileName", "This file is not a JSON file.");
        }

        if (curriculumName.IsWhiteSpace())
        {
            ModelState.AddModelError("FileName", "No curriculum was selected.");
        }

        if (ModelState.IsValid)
        {
            Stream readStream = file.OpenReadStream();
            StreamReader reader = new StreamReader(readStream);
            string fileContent = reader.ReadToEnd();
            
            reader.Close();
            readStream.Close();

            try
            {
                var lessons = JsonSerializer.Deserialize<List<Lesson.LessonDTO>>(fileContent);
                if (lessons == null)
                    throw new JsonException("null is not a valid curriculum list.");

                var curriculum = _curriculumRepo.GetByTitle(curriculumName);
                if (curriculum == null)
                    return NotFound();
                
                var result = _curriculumService.MergeLessons(curriculum._Lessons, lessons.Select(dto => dto.Lesson()).ToList());

                curriculum._Lessons = result;
                
                _curriculumService.Update(curriculum);
            }
            catch (JsonException e)
            {
                ModelState.AddModelError("FileName",
                    "The JSON file you uploaded is not a curriculum file: " + e.Message);
            }

            if (!ModelState.IsValid)
            {
                ViewBag.curriculumNames = _curriculumRepo.GetAllCurriculums().Select(c => c.Title);
                return View(file);
            }
            else
            {
                return RedirectToAction("Index", "Mentor");                
            }
        }
        ViewBag.curriculumNames = _curriculumRepo.GetAllCurriculums().Select(c => c.Title);
        return View(file);
    }
    
    // Code-Owner: Julia Sandner
    // GET: /Mentor/AssignmentOverview
    [HttpGet]
    public IActionResult AssignmentOverview(int traineeId)
    {        
        var trainee = _userRepo.GetById(traineeId) as Trainee;

        if (trainee == null)
        {
            return Unauthorized();
        }

        var assignments = _assignmentRepo.FindByTrainee(trainee);
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

        return View("AssignmentOverview", assignments);
    }

    //Code-Owner: Julia Sandner
    [HttpPost]
    public IActionResult Accept(int assignmentId)
    {
        var assignment = _assignmentRepo.GetById(assignmentId);
        if (assignment == null)
        {
            return NotFound();
        }

        _assignmentRepo.UpdateStatus(assignmentId, LessonAssignmentStatus.Accepted);

        return RedirectToAction("AssignmentOverview", new { traineeId = assignment.TraineeId });
    }

    // Code-Owner: Julia Sandner
    [HttpPost]
    public IActionResult SkipAssignment(int assignmentId)
    {
        var assignment =_assignmentRepo.GetById(assignmentId);
        if(assignment == null)
        {
            return NotFound();
        }
        _assignmentRepo.UpdateStatus(assignmentId, LessonAssignmentStatus.Skipped);
        return RedirectToAction("AssignmentOverview", new {traineeId = assignment.TraineeId});

    }    

    // Code-Owner: Julia Sandner
    public IActionResult UpdateAssignmentOrder(int traineeId, [FromForm] List<int> orderedIds)
    {
        _assignmentRepo.UpdateAssignmentPositions(orderedIds);
        return RedirectToAction("AssignmentOverview", new {traineeId});
    }

    // Code-Owner: Julia Sandner
    public IActionResult RejectAssignment(int assignmentId, string reason)
    {   var assignment = _assignmentRepo.GetById(assignmentId);
        if(assignment == null)
        {
            return NotFound();
        }

        if (string.IsNullOrWhiteSpace(reason))
        {
            ModelState.AddModelError(string.Empty, "A reason is required.");
            return RedirectToAction("AssignmentOverview", "Mentor", new {traineeId = assignment.TraineeId}); 
        }

        _rejectionRepo.Reject(assignmentId, reason);
        return RedirectToAction("AssignmentOverview", "Mentor", new {traineeId = assignment.TraineeId}); 
    }
}