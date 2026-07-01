// Code Owner: Jelena Cosic
using System.Collections.Generic;
using System.Net.Mime;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
  using Trainee_Tracker.Models;  

namespace Trainee_Tracker.Controllers;

[Authorize(Roles = "Mentor")]
public class MentorController : Controller
{
    /// <summary>
    /// Displays the Mentor dashboard.
    /// Only accessible by users with the Mentor role.
    /// </summary>
    /// <returns>The Mentor index view.</returns>
    public IActionResult Index() => View();

    /// <summary>
    /// Assigns a Mentor to a trainee.
    /// </summary>
    /// <param name="mentorId">The numeric id of the Mentor to assign.</param>
    /// <param name="traineeId">The numeric id of the Trainee to assign.</param>
    /// <author>Leon</author>
    /// <returns></returns>
    public IActionResult AssignTrainee(int mentorId, int traineeId)
    {
        return StatusCode(501, "Not implemented!");
    }

    /// <summary>
    /// Changes the order of lessons for a particular trainee.
    /// </summary>
    /// <param name="traineeId">The numeric id of the Trainee to change the order for.</param>
    /// <param name="order">A list with the numeric ids of Lessons in the order they should appear for trainees.</param>
    /// <author>Leon</author>
    /// <returns></returns>
    public IActionResult UpdateLessonOrder(int traineeId, IList<int> order)
    {
        return StatusCode(501, "Not implemented!");
    }

    [HttpGet]   // add this at the top if it isn't there

public IActionResult Fortschrittskontrolle()
{
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
    
    /// <author>Leon</author>
    [HttpGet]
    public IActionResult ImportCurriculum()
    {
        var curriculumNames = new List<String>();
        curriculumNames.Add("makandra Curriculum");
        curriculumNames.Add("makandra DevOps Curriculum");
        ViewBag.curriculumNames = curriculumNames;
        return View(null);
    }
    
    /// <author>Leon</author>
    [HttpPost]
    public IActionResult ImportCurriculum(string curriculumName, IFormFile file)
    {
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