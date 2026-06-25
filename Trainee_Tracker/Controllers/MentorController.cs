using System.Net.Mime;
using Microsoft.AspNetCore.Mvc;
using Trainee_Tracker.Models;

namespace Trainee_Tracker.Controllers;

public class MentorController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
    
    /// <summary>
    ///  Assigns a Mentor to a trainee.
    /// </summary>
    /// <param name="mentorId">The numeric id of the Mentor to assign.</param>
    /// <param name="traineeId">The numeric id of the Trainee to assign.</param>
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
    /// <returns></returns>
    public IActionResult UpdateLessonOrder(int traineeId, IList<int> order)
    {
        return StatusCode(501, "Not implemented!");
    }

    [HttpGet]
    public IActionResult ImportCurriculum()
    {
        return View(null);
    }

    [HttpPost]
    public IActionResult ImportCurriculum(IFormFile file)
    {
        if (file == null)
            ModelState.AddModelError("FileName", "No file was selected.");
        else
        {
            ContentType fileContentType = new ContentType(file.ContentType);
            if (fileContentType.MediaType != "application/json")
                ModelState.AddModelError("FileName", "This file is not a JSON file.");
        }
        if (ModelState.IsValid)
        {
            return RedirectToAction("Index", "Mentor");
        }
        return View(file);
    }
    
    public IActionResult GatherRecentFeedback()
    {
        var feedbacks = new List<LessonFeedback>();
        
        feedbacks.Add(new LessonFeedback()
        {
            Difficulty = 4,
            ActualEffort = 3,
            Comment = "Very good",
            PriorKnowledge = "None"
        });
        feedbacks.Add(new LessonFeedback()
        {
            Difficulty = 1,
            ActualEffort = 0.5,
            Comment = "Way to easy",
            PriorKnowledge = "Everything"
        });
        feedbacks.Add(new LessonFeedback()
        {
            Difficulty = 3,
            ActualEffort = 2,
            Comment = "Was ok",
            PriorKnowledge = "Javascrip basics"
        });

        ViewBag.feedbacks = feedbacks;
        return View();
    }
}