using Microsoft.AspNetCore.Mvc;
using Trainee_Tracker.Models;
using Trainee_Tracker.Services;

namespace Trainee_Tracker.Controllers;

public class FeedbackController : Controller
{
    private readonly LessonFeedbackService _lessonFeedbackService;

    public FeedbackController(LessonFeedbackService lessonFeedbackService)
    {
        _lessonFeedbackService = lessonFeedbackService;
    }

    /// <summary>
    /// Displays recent lesson feedbacks.
    /// </summary>
    /// <returns>The RecentFeedback view with a list of lesson feedbacks.</returns>
    [HttpGet]
    public IActionResult RecentFeedback(DateTime? from, DateTime? until)
    {
        var feedbacks = new List<LessonFeedback>();

        if (from.HasValue && until.HasValue)
        {
            feedbacks = _lessonFeedbackService.CollectFeedback(from.Value, until.Value);
        }

        ViewBag.From = from;
        ViewBag.Until = until;

        return View(feedbacks);
    }

    /// <summary>
    /// Displays the page where a trainee can create lesson feedback.
    /// </summary>
    /// <returns>The CreateFeedback view.</returns>
    [HttpGet]
    public IActionResult CreateFeedback()
    {
        return View();
    }

    [HttpPost]
    public IActionResult CreateFeedback(LessonFeedback feedback)
    {
        if (!ModelState.IsValid)
        {
            return View(feedback);
        }

        feedback.TraineeId = 1;
        feedback.MentorId = 2;
        feedback.CreatedAt = DateTime.UtcNow;

        _lessonFeedbackService.CreateFeedback(feedback);

        return RedirectToAction(nameof(RecentFeedback));
    }
}