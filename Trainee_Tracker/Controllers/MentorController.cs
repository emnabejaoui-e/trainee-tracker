// Code Owner: Jelena Cosic ([Authorize])
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
    private readonly IAssignmentService _assignmentService;

    public MentorController(
        IMentorRepository mentorRepo,
        IUserRepository userRepo,
        ILessonAssignmentRepository assignmentRepo,
        IRejectionRepository rejectionRepo,
        ITraineeRepository traineeRepo,
        WorkingHoursService workingHoursService,
        IProgressService progressService,
        ICurriculumRepository curriculumRepo,
        ICurriculumService curriculumService,
        IAssignmentService assignmentService
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
        _assignmentService = assignmentService;
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
            ViewBag.allTrainees = _traineeRepo.GetAllTrainees()
                .Where(t => !t.Closed);
        }
        
        return View(currentUser.AssignedTrainees.Where(t => !t.Closed));
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

    // Code-Owner: Leon
    // GET: /Mentor/ImportCurriculum
    [HttpGet]
    public IActionResult ImportCurriculum()
    {
        ViewData["NavbarOverride"] = "Mentor";
        ViewBag.curriculumNames = _curriculumRepo.GetAllCurriculums().Select(c => c.Title);
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


    // Code-Owner: Julia
    /// <summary>
    /// Displays an overview of the assignments of a given trainee for a mentor, including any rejection history for their assignments
    /// </summary>
    /// <param name="traineeId">numeric id of the trainee whose assignments should be shown</param>
    /// <returns>AssignmentOverview view or Unauthorized if the trainee does not exist</returns>
    [HttpGet]
    public IActionResult AssignmentOverview(int traineeId)
    {        
        var trainee = _userRepo.GetById(traineeId) as Trainee;

        if (trainee == null)
        {
            return Unauthorized();
        }
        var result = _assignmentService.GetOverview(trainee);

        ViewBag.RejectionReasons = result.RejectionHistory;
        ViewBag.AssignmentsWithHistory = result.AssignmentsWithHistory;

        return View("AssignmentOverview", result.Assignments);
    }




    //Code-Owner: Julia Sandner
    /// <summary>
    /// Marks a lesson assignment as accepted
    /// </summary>
    /// <param name="assignmentId"> the numeric id of the assignment to accept</param>
    /// <returns>a redirect to the ASsignmentOverview, 404 if not found or the overview with an error if the transition is invalid</returns>
    [HttpPost]
    public IActionResult Accept(int assignmentId)
    {
        var currentAssignment = _assignmentRepo.GetById(assignmentId);
        if (currentAssignment == null)
        {
            return NotFound();
        }

        try
        {
            var assignment = _assignmentService.UpdateAssignmentStatus(assignmentId, LessonAssignmentStatus.Accepted);
            return RedirectToAction("AssignmentOverview", new {traineeId = assignment!.TraineeId});
        }
        catch(InvalidOperationException e)
        {
            TempData["Error"] = e.Message;
            return RedirectToAction("AssignmentOverview", new { traineeId = currentAssignment.TraineeId });
            
        }
    }

    //Code-Owner: Julia Sandner
    /// <summary>
    /// Marks a lesson assignment as skipped
    /// </summary>
    /// <param name="assignmentId"> the numeric id of the assignment to skip</param>
    /// <returns>a redirect to the ASsignmentOverview, 404 if not found or the overview with an error if the transition is invalid</returns>
    [HttpPost]
    public IActionResult SkipAssignment(int assignmentId)
    {
        var currentAssignment =_assignmentRepo.GetById(assignmentId);
        if(currentAssignment == null)
        {
            return NotFound();
        }

        try
        {
            var assignment = _assignmentService.UpdateAssignmentStatus(assignmentId, LessonAssignmentStatus.Skipped);
            return RedirectToAction("AssignmentOverview", new {traineeId = assignment!.TraineeId});
        }
        catch(InvalidOperationException e)
        {
            TempData["Error"] = e.Message;
            return RedirectToAction("AssignmentOverview", new { traineeId = currentAssignment.TraineeId });
            
        }
    }    

    //Code-Owner: Julia Sandner
    /// <summary>
    /// Persists a new display order for a trainee's assignments
    /// </summary>
    /// <param name="traineeId"> the numeric id of the trainee whose assignments were reordered</param>
    /// <param name="orderedIds">the assignment ids in their new display order</param>
    /// <returns> a redirect to the AssignmentOverview for the given trainee</returns>
    public IActionResult UpdateAssignmentOrder(int traineeId, [FromForm] List<int> orderedIds)
    {
        _assignmentService.UpdateAssignmentOrder(orderedIds);
        return RedirectToAction("AssignmentOverview", new {traineeId});
    }


    //Code-Owner: Julia Sandner
    /// <summary>
    /// REjects an assignment with a reason
    /// </summary>
    /// <param name="assignmentId">numeric id of the assignment to reject</param>
    /// <param name="reason">reason for the rejection</param>
    /// <returns>redirect to the AssignmentOverview or Not-Found or the Overview with an error</returns>
    public IActionResult RejectAssignment(int assignmentId, string reason)
    {   var currentAssignment = _assignmentRepo.GetById(assignmentId);
        if(currentAssignment == null)
        {
            return NotFound();
        }

        try
        {
            _assignmentService.RejectAssignment(assignmentId, reason);
        }
        catch(ArgumentException e)
        {
            TempData["Error"] = e.Message;
        }
        catch(InvalidOperationException ex)
        {
            TempData["Error"] = ex.Message;   
        }

        return RedirectToAction("AssignmentOverview", "Mentor", new {traineeId = currentAssignment.TraineeId}); 
    }
}