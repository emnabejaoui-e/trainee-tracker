// Code Owner: Jelena Cosic ([Authorize])
using System.Diagnostics.Contracts;
using System.Net.Mime;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Trainee_Tracker.Data.LessonAssignments;
using Trainee_Tracker.Data.MentorRepository;
using Trainee_Tracker.Data.Rejections;
using Trainee_Tracker.Data.TraineeRepository;
using Trainee_Tracker.Models;
using Trainee_Tracker.Repositories;

namespace Trainee_Tracker.Controllers;

[Authorize(Roles = "Mentor, Admin")]
public class MentorController : Controller
{
    private IMentorRepository _mentorRepo;
    private IUserRepository _userRepo;
    private ILessonAssignmentRepository _assignmentRepo;
    private IRejectionRepository _rejectionRepo;
    private ITraineeRepository _traineeRepo;

    public MentorController(IMentorRepository mentorRepo, IUserRepository userRepo, ILessonAssignmentRepository assignmentRepo, IRejectionRepository rejectionRepo, ITraineeRepository traineeRepo)
    {
        _mentorRepo = mentorRepo;
        _userRepo = userRepo;
        _assignmentRepo = assignmentRepo;
        _rejectionRepo = rejectionRepo;
        _traineeRepo = traineeRepo;
    }

    // Code-Owner: Jelena Cosic
    // GET: /Mentor/Index
    /// <summary>
    /// Displays the Mentor dashboard.
    /// Only accessible by users with the Mentor or Admin role.
    /// </summary>
    /// <returns>The Mentor index view.</returns>
    public IActionResult Index() => RedirectToAction("MyTrainees");

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
        
        ViewBag.isAdmin = "Admin".Equals(User.FindFirstValue(ClaimTypes.Role));
        if (ViewBag.isAdmin)
        {
            ViewBag.allTrainees = _traineeRepo.GetAllTrainees();            
        }
        
        return View(currentUser.AssignedTrainees);
    }

    // Code-Owner: Leon
    // GET: /Mentor/Fortschrittskontrolle
    [HttpGet]
    public IActionResult Fortschrittskontrolle()
    {
        ViewData["NavbarOverride"] = "Mentor";
        var model = new ProgressControlData
        {
            DaysWorked = 15,
            Finished = 12,
            Open = 8,
            Buffer = 2,
            Speed = 110,
            PredictedBuffer = 3
        };
        return View(model);
    }

    // Code-Owner: Leon
    // GET: /Mentor/ImportCurriculum
    [HttpGet]
    public IActionResult ImportCurriculum()
    {
        ViewData["NavbarOverride"] = "Mentor";
        var curriculumNames = new List<String>();
        curriculumNames.Add("makandra Curriculum");
        curriculumNames.Add("makandra DevOps Curriculum");
        ViewBag.curriculumNames = curriculumNames;
        return View(null);
    }

    // Code-Owner: Leon
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
            return RedirectToAction("Index", "Mentor");
        }
        var curriculumNames = new List<String>();
        curriculumNames.Add("makandra Curriculum");
        curriculumNames.Add("makandra DevOps Curriculum");
        ViewBag.curriculumNames = curriculumNames;
        return View(file);
    }


    // Code-Owner: Julia
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

//Code-Owner: Julia 
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

//Code-Owner: Julia
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

    public IActionResult UpdateAssignmentOrder(int traineeId, [FromForm] List<int> orderedIds)
    {
        _assignmentRepo.UpdateAssignmentPositions(orderedIds);
        return RedirectToAction("AssignmentOverview", new {traineeId});
    }
}