using Microsoft.AspNetCore.Mvc;
using Trainee_Tracker.Models;

namespace Trainee_Tracker.Controllers;
public class FeedbackController : Controller
{
    /// <summary>
    /// Displays recent lesson feedbacks with temporary dummy data.
    /// </summary>
    /// <returns>The RecentFeedback view with a list of lesson feedbacks.</returns>
    public IActionResult RecentFeedback()
    {
        var feedbacks = new List<LessonFeedback>
        {
            new LessonFeedback
            {
                Difficulty = 4,
                ActualEffort = 3,
                Comment = "Very good",
                PriorKnowledge = "None"
            },
            new LessonFeedback
            {
                Difficulty = 5,
                ActualEffort = 0.5,
                Comment = "Way too easy",
                PriorKnowledge = "Everything"
            },
            new LessonFeedback
            {
                Difficulty = 3,
                ActualEffort = 2,
                Comment = "Was ok",
                PriorKnowledge = "JavaScript basics"
            }
        };

        return View(feedbacks);
    }

    /// <summary>
    /// Displays the page where a trainee can create lesson feedback.
    /// </summary>
    /// <returns>The CreateFeedback view.</returns>
    public IActionResult CreateFeedback()
    {
        return View();
    }
}