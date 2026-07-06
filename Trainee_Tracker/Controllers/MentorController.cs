// Code Owner: Jelena Cosic (Grundgerüst, [Authorize], Index)
using System.Diagnostics.Contracts;
using System.Net.Mime;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Trainee_Tracker.Data.MentorRepository;
using Trainee_Tracker.Models;

namespace Trainee_Tracker.Controllers;

[Authorize(Roles = "Mentor, Admin")]
public class MentorController : Controller
{
    private IMentorRepository _mentorRepo;

    public MentorController(IMentorRepository mentorRepo)
    {
        _mentorRepo = mentorRepo;
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
        string mentorEmail = User.Identities.First().Claims
            .First(cl => cl.Type == ClaimTypes.Email).Value;

        Mentor? currentUser = _mentorRepo.GetMentorByEMail(mentorEmail);

        Contract.Assert(currentUser != null, "Inconsistent login state (no/invalid email set).");

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
}